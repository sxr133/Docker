using System.ComponentModel.DataAnnotations;

namespace SportingStatsBackEnd.Models
{
    public class UserLoginRequest
    {
        public string Email { get; set; } // Matches "email" from the frontend
        public string Username { get; set; } // New property for username, if it's used instead of email
        [Required]
        public string Password { get; set; } // Matches "password" from the frontend
    }
}
