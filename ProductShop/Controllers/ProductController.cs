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
    public IEnumerable<Product> Get()
    {
        var products = _connection.Query<Product>("SELECT * FROM Product");

        return products;
    }


    // GET api/<ProductController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<ProductController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<ProductController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<ProductController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}