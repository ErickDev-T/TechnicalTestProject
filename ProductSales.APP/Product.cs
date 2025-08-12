using System.ComponentModel.DataAnnotations;
//using Microsoft.EntityFrameworkCore;

namespace ProductSales.APII.Models;

public class Product
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = default!;

    [MaxLength(500)]
    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }
}
