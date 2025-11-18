using Microsoft.AspNetCore.Mvc;
using LevelUpApi.Data;
using LevelUpApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LevelUpApi.Models.Dto;


namespace LevelUpApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Email e senha são obrigatórios.");

            if (_db.Users.Any(x => x.Email == dto.Email))
                return BadRequest("Email já está registrado.");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsAdmin = false
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            return Ok(new { message = "Conta criada com sucesso!" });
        }

    
        [HttpPost("login")]
public IActionResult Login([FromBody] LoginDto dto)
{
    var user = _db.Users.FirstOrDefault(x => x.Email == dto.Email);

    if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        return Unauthorized("Credenciais inválidas.");

    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim("isAdmin", user.IsAdmin.ToString())
    };

    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
    );

    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _config["Jwt:Issuer"],
        claims: claims,
        expires: DateTime.UtcNow.AddDays(7),
        signingCredentials: creds
    );

    return Ok(new
    {
        token = new JwtSecurityTokenHandler().WriteToken(token),
        user = new { user.Id, user.Name, user.Email, user.IsAdmin }
    });
}
    }
}
