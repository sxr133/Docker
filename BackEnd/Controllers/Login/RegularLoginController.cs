using System.IO;  // For StreamReader
using System.Text; // For Encoding
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SportingStatsBackEnd.Services;
using SportingStatsBackEnd.Models;
using AppModels = SportingStatsBackEnd.Models;
using RegisterRequest = SportingStatsBackEnd.Models.RegisterRequest;
using log4net;

namespace SportingStatsBackEnd.Controllers.User
{
    [Route("api/auth")]
    public class RegularLoginController : ControllerBase
    {
        private readonly IAuthService _authService;
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(RegularLoginController));

        public RegularLoginController(IAuthService authService)
        {
            _authService = authService;
            logger.Info("RegularLoginController initialized");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest model)
        {
            // Log headers and body for debugging
            var headers = Request.Headers;
            logger.Info("Headers: " + string.Join(", ", headers.Select(h => $"{h.Key}: {h.Value}")));

            HttpContext.Request.EnableBuffering();

            string body;
            using (var reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                body = await reader.ReadToEndAsync();
                HttpContext.Request.Body.Position = 0; // Reset the position
            }
            logger.Info($"Body: {body}");

            if (model == null)
            {
                logger.Error("The request body cannot be null.");
                return BadRequest("The request body cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest("Email and Password are required.");
            }


            if (!ModelState.IsValid)
            {
                logger.Warn($"Invalid login model state: {string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
                return BadRequest("Invalid data.");
            }

            var user = await _authService.AuthenticateAsync(model.Email, model.Password);
            if (user == null)
            {
                logger.Warn($"Authentication failed for {model.Email}. Invalid credentials.");
                return Unauthorized("Invalid email/username or password.");
            }

            var token = _authService.GenerateJwtToken(user);
            logger.Info($"Login successful for user: {model.Email}");
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            logger.Info($"Registration attempt for Email: {model.Email}, Username: {model.Username}");

            if (!ModelState.IsValid)
            {
                logger.Warn($"Invalid registration model state: {string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
                return BadRequest("Invalid data.");
            }

            var userExists = await _authService.UserExistsAsync(model.Email, model.Username);
            if (userExists)
            {
                logger.Warn($"Registration failed: User already exists with Email: {model.Email}");
                return Conflict("User already exists.");
            }

            var user = await _authService.RegisterAsync(model.Email, model.Username, model.Password);
            if (user == null)
            {
                logger.Error($"Registration failed for Email: {model.Email}, Username: {model.Username}");
                return BadRequest("Error during registration.");
            }

            var token = _authService.GenerateJwtToken(user);
            logger.Info($"Registration successful for Email: {model.Email}, Username: {model.Username}");
            return Ok(new { token });
        }
    }
}