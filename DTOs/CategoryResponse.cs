using Dot_Net_Basic.Models;

namespace Dot_Net_Basic.DTOs;

public record CategoryResponse(int Id, string Name, string? Description)
{
    public static CategoryResponse FromEntity(Category category)
        => new(category.Id, category.Name, category.Description);
}
