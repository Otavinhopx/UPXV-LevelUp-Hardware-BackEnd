using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LevelUpApi.Data;
using LevelUpApi.Models;
using System.Security.Claims;

namespace LevelUpApi.Controllers
{
    [ApiController]
    [Route("reviews")]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ReviewsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost("{productId}")]
        [Authorize]
        public IActionResult CreateReview(int productId, [FromBody] ReviewDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var user = _db.Users.Find(userId);
            if (user == null) return Unauthorized();

            var review = new Review
            {
                ProductId = productId,
                UserId = userId,
                Stars = dto.Stars,
                Comment = dto.Comment
            };

            _db.Reviews.Add(review);
            _db.SaveChanges();

            return Ok(new { message = "Review enviado!" });
        }

        [HttpGet("{productId}")]
        public IActionResult GetReviews(int productId)
        {
            var list = _db.Reviews
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new
                {
                    r.Id,
                    r.Stars,
                    r.Comment,
                    r.CreatedAt,
                    UserName = r.User!.Name
                })
                .ToList();

            return Ok(list);
        }
    }
}
