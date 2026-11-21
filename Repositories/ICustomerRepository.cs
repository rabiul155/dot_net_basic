using Dot_Net_Basic.Models;

namespace Dot_Net_Basic.Repositories;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    void Add(Customer customer);
    void Remove(Customer customer);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
