using Dot_Net_Basic.Models;

namespace Dot_Net_Basic.DTOs.Customers;

public record CustomerResponse(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    DateTime CreatedAtUtc)
{
    public static CustomerResponse FromEntity(Customer customer) => new(
        customer.Id,
        customer.FirstName,
        customer.LastName,
        customer.Email,
        customer.Phone,
        customer.CreatedAtUtc);
}
