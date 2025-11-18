namespace LevelUpApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Brand { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public string AffiliateUrl { get; set; } = "";
        public string? ImageUrl { get; set; }

        public List<Article>? Articles { get; set; }
        public List<Review>? Reviews { get; set; }
    }
}
