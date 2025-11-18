using System.ComponentModel.DataAnnotations;

namespace LevelUpApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; } = "";

        [Required]
        public string PasswordHash { get; set; } = "";

        public bool IsAdmin { get; set; }

        [Required]
        public string Name { get; set; } = "";

        public List<Review>? Reviews { get; set; }
    }
}
