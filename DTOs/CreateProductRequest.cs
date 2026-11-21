using System.ComponentModel.DataAnnotations;

namespace Dot_Net_Basic.DTOs;

public class CreateProductRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; init; }

    [Range(0, 10_000_000)]
    public decimal Price { get; init; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; init; }

    public int? CategoryId { get; init; }
}
