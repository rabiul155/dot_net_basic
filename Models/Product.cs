namespace Dot_Net_Basic.Models;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    /// <summary>Optional category of the product.</summary>
    public int? CategoryId { get; set; }
}
