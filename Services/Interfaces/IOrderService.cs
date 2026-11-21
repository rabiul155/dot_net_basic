using Dot_Net_Basic.DTOs.Orders;

namespace Dot_Net_Basic.Services.Interfaces;

public interface IOrderService
{
    Task<IReadOnlyList<OrderResponse>> GetAllAsync(int? customerId, CancellationToken cancellationToken = default);
    Task<OrderResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
