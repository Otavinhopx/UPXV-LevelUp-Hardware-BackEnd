using System.ComponentModel.DataAnnotations;


namespace LevelUpApi.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required] public string Email { get; set; } = null!;
        [Required] public string PasswordHash { get; set; } = null!;
        public bool IsAdmin { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}