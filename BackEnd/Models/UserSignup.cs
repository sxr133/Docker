using System.ComponentModel.DataAnnotations;

namespace SportingStatsBackEnd.Models
{
    public class UserSignup
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(15, ErrorMessage = "First Name must be between 1 and 15 characters", MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(15, ErrorMessage = "Last Name must be between 1 and 15 characters", MinimumLength = 1)]
        public string LastName { get; set; }

        public string StreetAddress1 { get; set; }

        public string StreetAddress2 { get; set; }

        public string City { get; set; }

        public string State_Prov { get; set; }

        public string Zip_Post_Cd { get; set; }

        public string Country { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters", MinimumLength = 6)]
        public string Password { get; set; }

        // Add the verification token property
        public string VerificationToken { get; set; }

        // Add IsEmailVerified token property
        public bool IsEmailVerified { get; set; }
    }
}