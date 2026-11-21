namespace Dot_Net_Basic.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }

    /// <summary>The product that was ordered.</summary>
    public int ProductId { get; set; }

    // Name and price are copied at the time of ordering,
    // so later product changes don't rewrite old orders.
    public required string ProductName { get; set; }
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }
}
