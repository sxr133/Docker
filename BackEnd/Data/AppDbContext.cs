using Microsoft.EntityFrameworkCore;
using SportingStatsBackEnd.Models;

namespace SportingStatsBackEnd.Data
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