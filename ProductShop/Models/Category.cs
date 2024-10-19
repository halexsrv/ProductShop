using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ProductShop.Models;

public class Category
{
    public int Id { get; set; }

    [Microsoft.Build.Framework.Required]
    [MinLength(5)]
    public string? Name { get; set; }
}