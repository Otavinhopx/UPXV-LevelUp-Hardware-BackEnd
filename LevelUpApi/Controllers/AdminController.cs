using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LevelUpApi.Data;
using LevelUpApi.Models;
using System.Security.Claims;

namespace LevelUpApi.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AdminController(AppDbContext db) { _db = db; }

        bool IsAdmin()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "isAdmin");
            return claim != null && bool.TryParse(claim.Value, out var v) && v;
        }

        // ===================== PRODUTOS =====================
        [HttpPost("products")]
        [Authorize]
        public IActionResult CreateProduct([FromBody] ProductDto dto)
        {
            if (!IsAdmin()) return Forbid();

            var product = new Product
            {
                Title = dto.Title ?? string.Empty,
                Brand = dto.Brand ?? string.Empty,
                Description = dto.Description ?? string.Empty,
                Price = dto.Price ?? 0m, // decimal
                AffiliateUrl = dto.AffiliateUrl ?? string.Empty,
                ImageUrl = dto.ImageUrl ?? string.Empty
            };

            _db.Products.Add(product);
            _db.SaveChanges();

            // Cria artigos/reviews automáticos para testes
            var sampleArticles = new List<Article>
            {
                new Article { Title = "Review inicial", Content = "Este produto é excelente!", ProductId = product.Id },
                new Article { Title = "Opinião do usuário", Content = "Bom custo-benefício.", ProductId = product.Id }
            };

            _db.Articles.AddRange(sampleArticles);
            _db.SaveChanges();

            return CreatedAtAction(null, new { id = product.Id });
        }

        [HttpDelete("products/{id}")]
        [Authorize]
        public IActionResult DeleteProduct(int id)
        {
            if (!IsAdmin()) return Forbid();
            var p = _db.Products.Find(id);
            if (p == null) return NotFound();
            _db.Products.Remove(p);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPut("products/{id}")]
        [Authorize]
        public IActionResult UpdateProduct(int id, [FromBody] ProductDto dto)
        {
            if (!IsAdmin()) return Forbid();
            var p = _db.Products.Find(id);
            if (p == null) return NotFound();
            p.Title = dto.Title ?? p.Title;
            p.Brand = dto.Brand ?? p.Brand;
            p.Description = dto.Description ?? p.Description;
            p.Price = dto.Price ?? p.Price;
            p.AffiliateUrl = dto.AffiliateUrl ?? p.AffiliateUrl;
            p.ImageUrl = dto.ImageUrl ?? p.ImageUrl;
            _db.SaveChanges();
            return Ok(p);
        }

        // ===================== ARTIGOS =====================
        [HttpGet("articles")]
        [Authorize]
        public IActionResult GetArticles()
        {
            if (!IsAdmin()) return Forbid();
            return Ok(_db.Articles.OrderByDescending(a => a.PublishedAt).ToList());
        }

        [HttpPost("articles")]
        [Authorize]
        public IActionResult CreateArticle([FromBody] ArticleDto dto)
        {
            if (!IsAdmin()) return Forbid();
            var a = new Article
            {
                Title = dto.Title ?? string.Empty,
                Content = dto.Content ?? string.Empty,
                Author = dto.Author
            };
            _db.Articles.Add(a);
            _db.SaveChanges();
            return CreatedAtAction(null, new { id = a.Id });
        }

        [HttpPut("articles/{id}")]
        [Authorize]
        public IActionResult UpdateArticle(int id, [FromBody] ArticleDto dto)
        {
            if (!IsAdmin()) return Forbid();
            var a = _db.Articles.Find(id);
            if (a == null) return NotFound();
            a.Title = dto.Title ?? a.Title;
            a.Content = dto.Content ?? a.Content;
            a.Author = dto.Author;
            _db.SaveChanges();
            return Ok(a);
        }

        [HttpDelete("articles/{id}")]
        [Authorize]
        public IActionResult DeleteArticle(int id)
        {
            if (!IsAdmin()) return Forbid();
            var a = _db.Articles.Find(id);
            if (a == null) return NotFound();
            _db.Articles.Remove(a);
            _db.SaveChanges();
            return NoContent();
        }

        // ===================== DTOs =====================
        public record ArticleDto(string Title, string Content, string? Author);
        public record ProductDto(string? Title, string? Brand, string? Description, decimal? Price, string? AffiliateUrl, string? ImageUrl);
    }
}
