using SportingStatsBackEnd.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using log4net;

namespace SportingStatsBackEnd.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        // Declare a static logger instance
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AuthService));

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            logger.Info("AuthService initialized");
        }

        public async Task<User> AuthenticateAsync(string emailOrUsername, string password)
        {
            logger.Info($"Authenticating user: {emailOrUsername }");

            if (string.IsNullOrWhiteSpace(emailOrUsername))
            {
                logger.Warn("Email or Username cannot be null or empty.");
                throw new ArgumentException("Email or Username cannot be null or empty.");
            }
            logger.Info("Validating by email");
            // Try to find user by email first
            var user = await _userManager.FindByEmailAsync(emailOrUsername);

            logger.Info("Do i get here");
            // If no user found by email, try by username
            if (user == null)
            {
                logger.Info("Validating by username");
                user = await _userManager.FindByNameAsync(emailOrUsername);
            }

            if (user == null)
            {
                logger.Warn($"User not found: {emailOrUsername}");
                return null;
            }

            // Check if the password is correct
            if (await _userManager.CheckPasswordAsync(user, password))
            {
                await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
                return user;
            }

            // Log incorrect password attempt
            logger.Warn($"Incorrect password for user: {emailOrUsername}");
            return null;
        }

        public async Task<User> AuthenticateAsyncByUsername(string username, string password)
        {
            logger.Info($"Authenticating user with Username: {username}");

            if (string.IsNullOrWhiteSpace(username))
            {
                logger.Warn("Username cannot be null or empty.");
                throw new ArgumentException("Username cannot be null or empty.");
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                logger.Warn($"User not found: {username}");
                return null;
            }

            if (await _userManager.CheckPasswordAsync(user, password))
            {
                await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
                return user;
            }

            logger.Warn($"Incorrect password for user: {username}");
            return null;
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecret = _configuration["Jwt:Secret"];
            
            if (string.IsNullOrEmpty(jwtSecret))
            {
                logger.Error("JWT Secret is missing in the configuration.");
                throw new InvalidOperationException("JWT Secret is not configured.");
            }

            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> UserExistsAsync(string email, string username)
        {
            var userByEmail = await _userManager.FindByEmailAsync(email);
            if (userByEmail != null) return true;

            var userByUsername = await _userManager.FindByNameAsync(username);
            return userByUsername != null;
        }

        // Implement RegisterAsync method
        public async Task<User> RegisterAsync(string email, string username, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                logger.Warn($"User with email {email} already exists.");
                return null; // User already exists
            }

            var newUser = new User
            {
                UserName = username,
                Email = email,
            };

            var result = await _userManager.CreateAsync(newUser, password);
            if (result.Succeeded)
            {
                logger.Info($"User registered successfully: {email}");
                return newUser;
            }

            // Log individual errors for better traceability
            foreach (var error in result.Errors)
            {
                logger.Error($"Registration error for {email}: {error.Description}");
            }

            return null; // Registration failed
        }
    }
}