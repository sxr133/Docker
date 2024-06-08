using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SportingStats.Controllers.Validation.UserLogin;
using SportingStats.Data;

namespace SportingStats.Controllers.User
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {

            // Validate email using EmailValidator class
            if (!EmailValidation.IsValidEmail(request.Email))
            {
                // Invalid email format
                return BadRequest(new { Message = "Invalid email address" });
            }

            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

                if (user != null)
                {
                    // User exists, but check if the password matches
                    if (user.Password == request.Password)
                    {
                        // Password matches, so user is authenticated
                        return Ok(new { Message = "Login successful" });
                    }
                    else
                    {
                        // Password does not match
                        return Unauthorized(new { Message = "Incorrect password" });
                    }
                }
                else
                {
                    // User does not exist
                    return Unauthorized(new { Message = "User does not exist" });
                }
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error: {ex.Message}");

                // Return a 500 Internal Server Error with an error message
                return StatusCode(500, new { Message = "An unexpected error occurred" });
            }
        }
    }
}