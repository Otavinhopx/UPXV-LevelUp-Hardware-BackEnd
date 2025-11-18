using Microsoft.EntityFrameworkCore;
using LevelUpApi.Models;

namespace LevelUpApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Article> Articles => Set<Article>();
        public DbSet<Review> Reviews => Set<Review>();
    }
}
