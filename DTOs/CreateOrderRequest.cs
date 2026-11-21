using System.ComponentModel.DataAnnotations;

namespace Dot_Net_Basic.DTOs;

public class CreateOrderRequest
{
    [Range(1, int.MaxValue)]
    public int CustomerId { get; init; }

    [Required]
    [MinLength(1, ErrorMessage = "An order must contain at least one item.")]
    public List<CreateOrderItemRequest> Items { get; init; } = new();
}

public class CreateOrderItemRequest
{
    [Range(1, int.MaxValue)]
    public int ProductId { get; init; }

    [Range(1, 1000)]
    public int Quantity { get; init; }
}
