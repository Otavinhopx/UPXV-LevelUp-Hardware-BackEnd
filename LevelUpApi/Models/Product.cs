using System.ComponentModel.DataAnnotations;

namespace LevelUpApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required] public string Title { get; set; } = string.Empty;
        [Required] public string Brand { get; set; } = string.Empty;
        [Required] public string Description { get; set; } = string.Empty;
        [Required] public decimal Price { get; set; } // usar decimal
        [Required] public string AffiliateUrl { get; set; } = string.Empty;
        [Required] public string ImageUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Adicionado CreatedAt

        public List<Article> Articles { get; set; } = new List<Article>();
    }
}
