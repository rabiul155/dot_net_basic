namespace Dot_Net_Basic.Models;

public class ProductUpdateDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
