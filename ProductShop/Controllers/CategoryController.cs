using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using ProductShop.Models;

namespace ProductShop.Controllers;

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
    public ActionResult Get()
    {
        var categories = _connection.Query<Category>("SELECT * FROM Category");

        return Ok(categories);
    }

    // GET api/<CategoryController>/5
    [HttpGet("{id}")]
    public ActionResult Get(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@id", id);
        var category = _connection.QueryFirstOrDefault<Category>("SELECT * FROM Category WHERE Id = @id", parameters);

        if (category == null) return NotFound();

        return Ok(category);
    }

    // POST api/<CategoryController>
    [HttpPost]
    public ActionResult Post([FromBody] Category category)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // execute select query to check if category already exists
        var existParameters = new DynamicParameters();
        existParameters.Add("@Name", category.Name);
        var existSql = "SELECT * FROM Category WHERE Name = @Name";
        var existCategory = _connection.Query<Category>(existSql, existParameters).FirstOrDefault();
        if (existCategory != null) return BadRequest(new { message = "Category already exists" });

        // return the id of the newly inserted category
        var parameters = new DynamicParameters();
        parameters.Add("@Name", category.Name);
        var sql = "INSERT INTO Category (Name) VALUES (@Name); SELECT CAST(SCOPE_IDENTITY() as int)";
        var newCategoryId = _connection.QueryFirstOrDefault<int>(sql, parameters);
        category.Id = newCategoryId;

        return CreatedAtAction(nameof(Get), new { id = newCategoryId }, category);
    }

    // PUT api/<CategoryController>/5
    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] Category category)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // execute select query to check if category already exists
        var existParameters = new DynamicParameters();
        existParameters.Add("@Name", category.Name);
        var existSql = "SELECT * FROM Category WHERE Name = @Name";
        var existCategory = _connection.Query<Category>(existSql, existParameters).FirstOrDefault();
        if (existCategory != null) return BadRequest(new { message = "Category already exists" });

        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);
        parameters.Add("@Name", category.Name);
        var sql = "UPDATE Category SET Name = @Name WHERE Id = @Id";
        _connection.Execute(sql, parameters);

        return Ok();
    }

    // DELETE api/<CategoryController>/5
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        // execute select query to check if category exists
        var existParameters = new DynamicParameters();
        existParameters.Add("@Id", id);
        var existSql = "SELECT * FROM Category WHERE Id = @Id";
        var existCategory = _connection.Query<Category>(existSql, existParameters).FirstOrDefault();
        if (existCategory == null) return BadRequest(new { message = "Category does not exist" });

        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);
        var sql = "DELETE FROM Category WHERE Id = @Id";
        _connection.Execute(sql, parameters);

        return Ok();
    }
}