using Dot_Net_Basic.Models;

namespace Dot_Net_Basic.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync(int? customerId, CancellationToken cancellationToken = default);
    Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    void Add(Order order);
    void Remove(Order order);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
