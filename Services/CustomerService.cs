using Dot_Net_Basic.Helpers;
using Dot_Net_Basic.DTOs;
using Dot_Net_Basic.Models;
using Dot_Net_Basic.Repositories.Interfaces;
using Dot_Net_Basic.Services.Interfaces;

namespace Dot_Net_Basic.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IReadOnlyList<CustomerResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var customers = await _customerRepository.GetAllAsync(cancellationToken);
        return customers.Select(CustomerResponse.FromEntity).ToList();
    }

    public async Task<CustomerResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var customer = await GetExistingAsync(id, cancellationToken);
        return CustomerResponse.FromEntity(customer);
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        => _customerRepository.ExistsAsync(id, cancellationToken);

    public async Task<CustomerResponse> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var customer = new Customer
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email.Trim().ToLowerInvariant(),
            Phone = request.Phone,
            CreatedAtUtc = DateTime.UtcNow
        };

        _customerRepository.Add(customer);
        await _customerRepository.SaveChangesAsync(cancellationToken);

        return CustomerResponse.FromEntity(customer);
    }

    public async Task UpdateAsync(int id, UpdateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var customer = await GetExistingAsync(id, cancellationToken);

        customer.FirstName = request.FirstName;
        customer.LastName = request.LastName;
        customer.Email = request.Email.Trim().ToLowerInvariant();
        customer.Phone = request.Phone;

        await _customerRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var customer = await GetExistingAsync(id, cancellationToken);

        _customerRepository.Remove(customer);
        await _customerRepository.SaveChangesAsync(cancellationToken);
    }

    private async Task<Customer> GetExistingAsync(int id, CancellationToken cancellationToken)
        => await _customerRepository.GetByIdAsync(id, cancellationToken)
           ?? throw new NotFoundException(nameof(Customer), id);
}
