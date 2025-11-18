using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using LevelUpApi.Data;
using LevelUpApi.Models;

namespace LevelUpApi.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProductsController(AppDbContext db) { _db = db; }

       
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _db.Products.OrderByDescending(p => p.Id).ToList();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var p = _db.Products.Find(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpGet("{id}/articles")]
        public IActionResult GetArticlesForProduct(int id)
        {
            var articles = _db.Articles
                .Where(a => a.ProductId == id)
                .OrderByDescending(a => a.Id)
                .ToList();

            return Ok(articles);
        }

       
        [HttpGet("{id}/reviews")]
        public IActionResult GetReviews(int id)
        {
            var reviews = _db.Reviews
                .Where(r => r.ProductId == id)
                .OrderByDescending(r => r.Id)
                .Select(r => new
                {
                    r.Id,
                    r.Stars,
                    r.Comment,
                    UserName = r.User.Name
                })
                .ToList();

            return Ok(reviews);
        }

        [HttpPost("{id}/reviews")]
        [Authorize]
        public IActionResult CreateReview(int id, [FromBody] ReviewDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var userId = int.Parse(userIdClaim);
            var user = _db.Users.Find(userId);
            if (user == null) return Unauthorized();

            var review = new Review
            {
                ProductId = id,
                UserId = userId,
                Stars = dto.Stars,
                Comment = dto.Comment
            };

            _db.Reviews.Add(review);
            _db.SaveChanges();

            return Ok(new { message = "Review enviado!" });
        }
    }
}
