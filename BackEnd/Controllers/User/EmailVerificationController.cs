using Microsoft.AspNetCore.Mvc;
using SportingStatsBackEnd.Data;

namespace SportingStatsBackEnd.Controllers.User
{
    [ApiController]
    [Route("[controller]")]
    public class EmailVerificationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmailVerificationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("verify")]
        public IActionResult VerifyEmail([FromQuery] string token)
        {
            try
            {
                // Find the user with the provided verification token
                var user = _context.UserSignups.FirstOrDefault(u => u.VerificationToken == token);

                if (user == null)
                {
                    // User not found or token is invalid
                    return BadRequest(new { Message = "Invalid verification token" });
                }

                // Mark the user's email as verified
                user.IsEmailVerified = true;
                _context.SaveChanges();

                return Ok(new { Message = "Email verified successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { Message = "An unexpected error occurred" });
            }
        }
    }
}