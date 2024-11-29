using System.ComponentModel.DataAnnotations;

namespace SportingStatsBackEnd.Models
{
    public class RegisterRequest
    {

        public string Email { get; set; }


        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
