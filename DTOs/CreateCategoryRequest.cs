using System.ComponentModel.DataAnnotations;

namespace Dot_Net_Basic.DTOs;

public class CreateCategoryRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; init; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; init; }
}
