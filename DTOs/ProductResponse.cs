using Dot_Net_Basic.Models;

namespace Dot_Net_Basic.DTOs;

/// <summary>What the API returns. We never return the database entity directly.</summary>
public record ProductResponse(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    int Quantity,
    int? CategoryId)
{
    public static ProductResponse FromEntity(Product product) => new(
        product.Id,
        product.Name,
        product.Description,
        product.Price,
        product.Quantity,
        product.CategoryId);
}
