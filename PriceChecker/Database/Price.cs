using System.ComponentModel.DataAnnotations;

namespace PriceChecker.Database;

public class Price
{
    [Key]
    public DateTime DateId { get; set; }
    public string? Name { get; set; }
    public double Close { get; set; }
}