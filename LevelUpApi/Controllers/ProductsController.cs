using Microsoft.AspNetCore.Mvc;
using LevelUpApi.Data;

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
            var list = _db.Products.OrderByDescending(p => p.CreatedAt).ToList();
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
    }
}
