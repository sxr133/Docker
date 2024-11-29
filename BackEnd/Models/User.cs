using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SportingStatsBackEnd.Models
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}