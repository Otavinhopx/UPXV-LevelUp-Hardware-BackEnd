using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LevelUpApi.Data;
using LevelUpApi.Models;

namespace LevelUpApi.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        private bool IsAdmin()
        {
            return bool.TryParse(
                User.Claims.FirstOrDefault(c => c.Type == "isAdmin")?.Value,
                out var isAdmin
            ) && isAdmin;
        }

        // ======================= PRODUCTS =======================
        [HttpPost("products")]
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto dto)
        {
            if (!IsAdmin()) return Forbid();

            var product = new Product
            {
                Title = dto.Title ?? "",
                Brand = dto.Brand ?? "",
                Description = dto.Description ?? "",
                Price = dto.Price ?? 0m,
                AffiliateUrl = dto.AffiliateUrl ?? "",
                ImageUrl = dto.ImageUrl ?? ""
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            // Optional example articles
            var sampleArticles = new List<Article>
            {
                new Article { Title = "Review inicial", Content = "Este produto é excelente!", ProductId = product.Id, PublishedAt = DateTime.UtcNow },
                new Article { Title = "Opinião do usuário", Content = "Bom custo-benefício.", ProductId = product.Id, PublishedAt = DateTime.UtcNow }
            };

            _db.Articles.AddRange(sampleArticles);
            await _db.SaveChangesAsync();

            return Created($"/products/{product.Id}", product);
        }

        [HttpDelete("products/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!IsAdmin()) return Forbid();

            var p = await _db.Products.FindAsync(id);
            if (p == null) return NotFound();

            _db.Products.Remove(p);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("products/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto dto)
        {
            if (!IsAdmin()) return Forbid();

            var p = await _db.Products.FindAsync(id);
            if (p == null) return NotFound();

            p.Title = dto.Title ?? p.Title;
            p.Brand = dto.Brand ?? p.Brand;
            p.Description = dto.Description ?? p.Description;
            p.Price = dto.Price ?? p.Price;
            p.AffiliateUrl = dto.AffiliateUrl ?? p.AffiliateUrl;
            p.ImageUrl = dto.ImageUrl ?? p.ImageUrl;

            await _db.SaveChangesAsync();

            return Ok(p);
        }

        // ======================= ARTICLES =======================
        [HttpGet("articles")]
        [Authorize]
        public async Task<IActionResult> GetArticles()
        {
            if (!IsAdmin()) return Forbid();

            var list = await _db.Articles
                .Include(a => a.Product)
                .OrderByDescending(a => a.PublishedAt)
                .ToListAsync();

            return Ok(list);
        }

        [HttpPost("articles")]
        [Authorize]
        public async Task<IActionResult> CreateArticle([FromBody] ArticleDto dto)
        {
            if (!IsAdmin()) return Forbid();

            if (dto.ProductId.HasValue)
            {
                var exists = await _db.Products.AnyAsync(p => p.Id == dto.ProductId.Value);
                if (!exists)
                    return BadRequest(new { error = "ProductId inválido" });
            }

            var a = new Article
            {
                Title = dto.Title ?? "",
                Content = dto.Content ?? "",
                Author = dto.Author ?? "",
                ProductId = dto.ProductId ?? 0,
                PublishedAt = DateTime.UtcNow
            };

            _db.Articles.Add(a);
            await _db.SaveChangesAsync();

            return Created($"/articles/{a.Id}", a);
        }

        [HttpPut("articles/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateArticle(int id, [FromBody] ArticleDto dto)
        {
            if (!IsAdmin()) return Forbid();

            var a = await _db.Articles.FindAsync(id);
            if (a == null) return NotFound();

            if (dto.ProductId.HasValue)
            {
                var exists = await _db.Products.AnyAsync(p => p.Id == dto.ProductId.Value);
                if (!exists)
                    return BadRequest(new { error = "ProductId inválido" });

                a.ProductId = dto.ProductId.Value;
            }

            a.Title = dto.Title ?? a.Title;
            a.Content = dto.Content ?? a.Content;
            a.Author = dto.Author ?? a.Author;

            await _db.SaveChangesAsync();

            return Ok(a);
        }

        [HttpDelete("articles/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            if (!IsAdmin()) return Forbid();

            var a = await _db.Articles.FindAsync(id);
            if (a == null) return NotFound();

            _db.Articles.Remove(a);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // ======================= DTOs =======================
        public record ArticleDto(string? Title, string? Content, string? Author, int? ProductId);
        public record ProductDto(string? Title, string? Brand, string? Description, decimal? Price, string? AffiliateUrl, string? ImageUrl);
    }
}
