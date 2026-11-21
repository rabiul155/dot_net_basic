using Dot_Net_Basic.Models;

namespace Dot_Net_Basic.DTOs.Orders;

public record OrderResponse(
    int Id,
    int CustomerId,
    DateTime CreatedAtUtc,
    decimal TotalAmount,
    IReadOnlyList<OrderItemResponse> Items)
{
    public static OrderResponse FromEntity(Order order) => new(
        order.Id,
        order.CustomerId,
        order.CreatedAtUtc,
        order.TotalAmount,
        order.Items.Select(OrderItemResponse.FromEntity).ToList());
}

public record OrderItemResponse(
    int ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal)
{
    public static OrderItemResponse FromEntity(OrderItem item) => new(
        item.ProductId,
        item.ProductName,
        item.UnitPrice,
        item.Quantity,
        item.UnitPrice * item.Quantity);
}
