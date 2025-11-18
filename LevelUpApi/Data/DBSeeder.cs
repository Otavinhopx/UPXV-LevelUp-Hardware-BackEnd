using LevelUpApi.Models;
using BCryptNet = BCrypt.Net.BCrypt;

namespace LevelUpApi.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
           
            if (!db.Users.Any())
            {
                var admin = new User
                {
                    Name = "Administrador",
                    Email = "admin@levelup.local",
                    PasswordHash = BCryptNet.HashPassword("admin123"),
                    IsAdmin = true
                };

                db.Users.Add(admin);
                db.SaveChanges();
            }

          
            if (!db.Products.Any())
            {
                db.Products.Add(new Product
                {
                    Title = "Exemplo GPU 8GB",
                    Brand = "GPUBrand",
                    Description = "Placa de vídeo exemplo",
                    Price = 1299.90M,
                    AffiliateUrl = "https://exemplo.com/compra",
                    ImageUrl = ""
                });

                db.Products.Add(new Product
                {
                    Title = "SSD 1TB",
                    Brand = "FastStorage",
                    Description = "SSD NVMe 1TB",
                    Price = 399.50M,
                    AffiliateUrl = "https://exemplo.com/ssd",
                    ImageUrl = ""
                });

                db.SaveChanges();
            }

           
            if (!db.Articles.Any())
            {
                var product = db.Products.First();

                db.Articles.Add(new Article
                {
                    Title = "Como escolher uma GPU",
                    Content = "Conteúdo de exemplo sobre GPUs",
                    Author = "Equipe LevelUp",
                    ProductId = product.Id
                });

                db.SaveChanges();
            }

        }
    }
}
