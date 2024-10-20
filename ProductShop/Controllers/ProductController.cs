using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using ProductShop.Models;

namespace ProductShop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IDbConnection _connection;

    public ProductController(IDbConnection connection)
    {
        _connection = connection;
    }

    // GET: api/<ProductController>
    [HttpGet]
    public ActionResult Get()
    {
        var products = _connection.Query<Product>("SELECT * FROM Product");

        return Ok(products);
    }


    // GET api/<ProductController>/5
    [HttpGet("{id}")]
    public ActionResult Get(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@id", id);
        var product = _connection.QueryFirstOrDefault<Product>("SELECT * FROM Product WHERE Id = @id", parameters);

        if (product == null) return NotFound();

        return Ok(product);
    }

    // POST api/<ProductController>
    [HttpPost]
    public ActionResult Post([FromBody] Product product)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // execute select query to check if product already exists
        var existParameters = new DynamicParameters();
        existParameters.Add("@Name", product.Name);

        var existSql = "SELECT * FROM Product WHERE Name = @Name";

        var existProduct = _connection.Query<Product>(existSql, existParameters).FirstOrDefault();
        if (existProduct != null) return BadRequest(new { message = "Product already exists" });

        // return the id of the newly inserted product
        var parameters = new DynamicParameters();
        parameters.Add("@Name", product.Name);
        parameters.Add("@CategoryId", product.CategoryId);
        var sql =
            "INSERT INTO Product (Name, CategoryId) VALUES (@Name, @CategoryId); SELECT CAST(SCOPE_IDENTITY() as int)";
        var newProductId = _connection.QueryFirstOrDefault<int>(sql, parameters);
        product.Id = newProductId;

        return CreatedAtAction(nameof(Get), new { id = newProductId }, product);
    }

    // PUT api/<ProductController>/5
    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] Product product)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // execute select query to check if product already exists
        var existParameters = new DynamicParameters();
        existParameters.Add("@Name", product.Name);

        var existSql = "SELECT * FROM Product WHERE Name = @Name";

        var existProduct = _connection.Query<Product>(existSql, existParameters).FirstOrDefault();
        if (existProduct != null) return BadRequest(new { message = "Product already exists" });

        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);
        parameters.Add("@Name", product.Name);
        parameters.Add("@CategoryId", product.CategoryId);
        var sql = "UPDATE Product SET Name = @Name, CategoryId = @CategoryId WHERE Id = @Id";
        _connection.Execute(sql, parameters);

        return Ok();
    }

    // DELETE api/<ProductController>/5
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        // execute select query to check if product exists
        var existParameters = new DynamicParameters();
        existParameters.Add("@Id", id);
        var existSql = "SELECT * FROM Product WHERE Id = @Id";

        var existProduct = _connection.Query<Product>(existSql, existParameters).FirstOrDefault();
        if (existProduct == null) return NotFound(new { message = "Product not found" });

        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);
        var sql = "DELETE FROM Product WHERE Id = @Id";
        _connection.Execute(sql, parameters);

        return Ok();
    }
}