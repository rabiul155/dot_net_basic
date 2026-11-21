using System.ComponentModel.DataAnnotations;

namespace Dot_Net_Basic.DTOs.Customers;

public class CreateCustomerRequest
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; init; } = string.Empty;

    [Phone]
    [MaxLength(30)]
    public string? Phone { get; init; }
}
