using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportingStatsBackEnd.Models;

namespace SportingStatsBackEnd.Data
{
    public class AppDbContext : IdentityDbContext<User> // Ensure it uses the User class for Identity
    {
        // Constructor to pass DbContextOptions to the base constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Define your DbSets for entities
        public new DbSet<User> Users { get; set; }
        // Add other DbSets for additional entities
    }
}
