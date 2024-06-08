using Microsoft.EntityFrameworkCore;
using SportingStats.Models;

namespace SportingStats.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserSignup> UserSignups { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}