using Microsoft.AspNetCore.Mvc;
using LevelUpApi.Data;


namespace LevelUpApi.Controllers
{
    [ApiController]
    [Route("articles")]
    public class ArticlesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ArticlesController(AppDbContext db) { _db = db; }


        [HttpGet]
        public IActionResult GetAll() => Ok(_db.Articles.OrderByDescending(a => a.PublishedAt).ToList());


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var a = _db.Articles.Find(id);
            if (a == null) return NotFound();
            return Ok(a);
        }
    }
}