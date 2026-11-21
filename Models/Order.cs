namespace Dot_Net_Basic.Models;

public class Order
{
    public int Id { get; set; }

    /// <summary>The customer who placed the order.</summary>
    public int CustomerId { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public decimal TotalAmount { get; set; }

    public List<OrderItem> Items { get; set; } = new();
}
