using LevelUpApi.Data;
using LevelUpApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCryptNet = BCrypt.Net.BCrypt;


namespace LevelUpApi.Services
{
    public class AuthService
    {
        private readonly IConfiguration _cfg;
        private readonly AppDbContext _db;
        public AuthService(IConfiguration cfg, AppDbContext db) { _cfg = cfg; _db = db; }


        public string HashPassword(string plain) => BCryptNet.HashPassword(plain);
        public bool Verify(string plain, string hash) => BCryptNet.Verify(plain, hash);


        public string GenerateJwt(User user)
        {
            var key = _cfg["Jwt:Key"] ?? "dev_key";
            var issuer = _cfg["Jwt:Issuer"] ?? "LevelUpApi";
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var claims = new[] {
new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
new Claim(JwtRegisteredClaimNames.Email, user.Email),
new Claim("isAdmin", user.IsAdmin.ToString())
};
            var token = new JwtSecurityToken(issuer, issuer, claims, expires: DateTime.UtcNow.AddDays(7), signingCredentials: new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}