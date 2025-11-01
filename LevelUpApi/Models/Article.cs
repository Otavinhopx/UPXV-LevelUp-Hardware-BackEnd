using System.ComponentModel.DataAnnotations;

namespace LevelUpApi.Models
{
    public class Article
    {
        public int Id { get; set; }
        [Required] public string Title { get; set; } = string.Empty;
        [Required] public string Content { get; set; } = string.Empty;
        public string? Author { get; set; }
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
