using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SportingStatsBackEnd.Data;
using Microsoft.OpenApi.Models;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure logging
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        // Add configuration files based on environment
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        Console.WriteLine($"Environment is set to: {builder.Environment.EnvironmentName}");

        ConfigureServices(builder.Services, builder.Configuration, builder.Environment);
        var app = builder.Build();

        ConfigureApp(app, app.Environment);

        // Change the port based on the environment
        //string url = app.Environment.IsDevelopment() ? "https://*:5000" : "https://*:5001";
        //string url = "https://*:5000";
        //Console.WriteLine($"Url is set to: {url}");
        //app.Run(url);
        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddControllers();
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddHttpClient();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        });
        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdminRole", policy =>
            {
                policy.RequireRole("Admin");
            });
        });

        ConfigureCors(services, configuration);
    }

    private static void ConfigureCors(IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder =>
                {
                    builder.WithOrigins("http://localhost:8080")  //for Production -> .WithOrigins(allowedOrigins)
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "swagger"; // Set Swagger UI at the root
            });
        }
        else
        {
            Console.WriteLine("Production mode: Enabling Exception Handler and HSTS");
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseCors("AllowSpecificOrigin");

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
