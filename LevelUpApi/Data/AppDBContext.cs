using Microsoft.EntityFrameworkCore;
using LevelUpApi.Models;

namespace LevelUpApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Article> Articles => Set<Article>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relação: Product tem muitos Articles
            modelBuilder.Entity<Article>()
                .HasOne(a => a.Product)
                .WithMany(p => p.Articles)
                .HasForeignKey(a => a.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // quando produto for deletado, deleta os artigos
        }
    }
}
