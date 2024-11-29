using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SportingStatsBackEnd.Data;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using SportingStatsBackEnd.Services;
using SportingStatsBackEnd.Models;
using System.Text; // For Encoding
using Microsoft.AspNetCore.Authentication.JwtBearer; // For JwtBearerDefaults
using Microsoft.IdentityModel.Tokens; // For token validation parameters
using log4net;
using log4net.Config;
using Microsoft.Extensions.Logging;
using DotNetEnv;


public class Program
{
    // Declare a static logger instance
    private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Program));

    public static void Main(string[] args)
    {
        // Load .env file
        DotNetEnv.Env.Load();

        var builder = WebApplication.CreateBuilder(args);

        // Configure log4net
        var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
        log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));

        // Add log4net as a logging provider (without clearing existing providers)
        builder.Logging.AddLog4Net();

        // Add services to the container
        var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
        if (string.IsNullOrEmpty(jwtSecret))
        {
            throw new ArgumentNullException("JWT_SECRET_KEY", "JWT secret key is not set.");
        }
        var key = Encoding.ASCII.GetBytes(jwtSecret);

        // Add configuration files based on environment
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        Console.WriteLine($"Environment is set to: {builder.Environment.EnvironmentName}");

        // Configure services
        ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

        var app = builder.Build();

        ConfigureApp(app, app.Environment);

        logger.Info("----------Application Started----------");

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        logger.Info("Configure Services");
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

        // Load database configuration from environment variables
        string dbServer = Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost";
        string dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "1433";
        string dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "sportingstats";
        string dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "sa";
        string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "Wabush#002";
        string trustServerCertificate = Environment.GetEnvironmentVariable("TRUST_SERVER_CERTIFICATE") ?? "false"; // Default to false if not set

        // Validate environment variables
        if (string.IsNullOrEmpty(dbServer) || string.IsNullOrEmpty(dbPort) ||
            string.IsNullOrEmpty(dbName) || string.IsNullOrEmpty(dbUser) ||
            string.IsNullOrEmpty(dbPassword))
        {
            throw new InvalidOperationException("Missing required database environment variables.");
        }

        // Build the connection string
        string connectionString = $"Server={dbServer},{dbPort};Database={dbName};User Id={dbUser};Password={dbPassword}";

        // Append TrustServerCertificate if it's set to "true" in environment variables
        if (trustServerCertificate.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
            connectionString += ";TrustServerCertificate=true;";
        }

        // Test database connection
        SportingStatsBackEnd.Test.DatabaseTester.TestDatabaseConnection(connectionString);

        // Log connection details for debugging (exclude password)
        logger.Info("Database connection initialized:");
        logger.Info($"DB_SERVER: {dbServer}, DB_PORT: {dbPort}");
        logger.Info($"DB_NAME: {dbName}");
        logger.Info($"Connection String: {connectionString}");
    
        // Database Configuration (Use MySQL for example, adjust as needed)
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        // Add Identity services
        services.AddIdentity<User, IdentityRole>(options =>
        {
            // Configure password, lockout, etc., if needed
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        // Configure JWT Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY")))
            };
        });

        // Register custom services here
        services.AddScoped<IAuthService, AuthService>();  // Register AuthService as the implementation of IAuthService

        // Configure external authentication (Google, Facebook)
        services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
                options.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
            })
            .AddFacebook(options =>
            {
                options.AppId = Environment.GetEnvironmentVariable("FACEBOOK_APP_ID");
                options.AppSecret = Environment.GetEnvironmentVariable("FACEBOOK_APP_SECRET");
            });

        // Register Swagger services
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "SportingStats API", Version = "v1" });
        });

        // Add Authorization Policy
        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdminRole", policy =>
            {
                policy.RequireRole("Admin");
            });
        });

        // CORS Configuration (Allow frontend on localhost:8080 for development)
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
         builder =>
         {
             builder.WithOrigins("http://127.0.0.1:8080")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); // For cookies/authentication
         });
        });

        services.AddHttpClient();
    }

    private static void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env)
    {
        Console.WriteLine($"Running in {env.EnvironmentName} mode");

        if (env.IsDevelopment())
        {
            Console.WriteLine("Development mode: Enabling Swagger and Developer Exception Page");
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SportingStats API V1");
                c.RoutePrefix = "swagger"; // Set Swagger UI at the root
            });
        }
        else
        {
            Console.WriteLine("Production mode: Enabling Exception Handler and HSTS");
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        // UseRouting must come before UseAuthorization and UseEndpoints
        app.UseRouting();

        app.UseCors("AllowSpecificOrigin");
        if (env.IsProduction())
        {
            app.UseHttpsRedirection();
        }
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
