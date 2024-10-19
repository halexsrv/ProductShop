using System.Data;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductShop.Models;

namespace ProductShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IDbConnection _connection;

        public CategoryController(IDbConnection connection)
        {
            _connection = connection;
        }
        
        // GET: api/<CategoryController>
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            var categories = _connection.Query<Category>("SELECT * FROM Category");

            return categories;
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public Category Get(int id)
        {
            var parameters = new { Id = id };
            var sql = "SELECT * FROM Category WHERE Id = @id";
            var category = _connection.Query<Category>(sql, parameters).FirstOrDefault();
            
            return category;
        }

        // POST api/<CategoryController>
        [HttpPost]
        public ActionResult Post([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parameters = new { Name = category.Name };
            var sql = "INSERT INTO Category(Name) VALUES(@Name)";
            var result = _connection.Query<Category>(sql, parameters).FirstOrDefault();
            
            Console.WriteLine(result);

            return Ok();
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
