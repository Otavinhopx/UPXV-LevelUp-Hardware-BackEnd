using Microsoft.AspNetCore.Mvc;
using LevelUpApi.Data;
using LevelUpApi.Models;
using LevelUpApi.Services;


namespace LevelUpApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly AuthService _auth;
        public AuthController(AppDbContext db, AuthService auth) { _db = db; _auth = auth; }


        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            if (_db.Users.Any(u => u.Email == dto.Email)) return BadRequest(new { error = "Email já cadastrado" });
            var user = new User { Email = dto.Email, PasswordHash = _auth.HashPassword(dto.Password), IsAdmin = false };
            _db.Users.Add(user);
            _db.SaveChanges();
            return CreatedAtAction(nameof(Register), new { id = user.Id });
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var user = _db.Users.SingleOrDefault(u => u.Email == dto.Email);
            if (user == null) return Unauthorized(new { error = "Credenciais inválidas" });
            if (!_auth.Verify(dto.Password, user.PasswordHash)) return Unauthorized(new { error = "Credenciais inválidas" });
            var token = _auth.GenerateJwt(user);
            return Ok(new { token });
        }


        public record RegisterDto(string Email, string Password);
        public record LoginDto(string Email, string Password);
    }
}