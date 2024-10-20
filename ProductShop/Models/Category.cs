using System.ComponentModel.DataAnnotations;

namespace ProductShop.Models;

public class Category
{
    public int Id { get; set; }

    [Required]
    [MinLength(5)]
    public string? Name { get; set; }
}