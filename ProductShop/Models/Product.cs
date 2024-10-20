﻿using System.ComponentModel.DataAnnotations;

namespace ProductShop.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    [MinLength(5)]
    public string? Name { get; set; }

    [Required]
    public int CategoryId { get; set; }
}