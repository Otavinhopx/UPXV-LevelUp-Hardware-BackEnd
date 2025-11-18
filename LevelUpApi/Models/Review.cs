using System.ComponentModel.DataAnnotations;

namespace LevelUpApi.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Range(1, 5)]
        public int Stars { get; set; }

        [Required]
        public string Comment { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Product? Product { get; set; }
        public User? User { get; set; }
    }
}
