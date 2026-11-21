using Dot_Net_Basic.Helpers;
using Dot_Net_Basic.DTOs;
using Dot_Net_Basic.Models;
using Dot_Net_Basic.Repositories.Interfaces;
using Dot_Net_Basic.Services.Interfaces;

namespace Dot_Net_Basic.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerService _customerService;
    private readonly IProductService _productService;

    public OrderService(IOrderRepository orderRepository, ICustomerService customerService, IProductService productService)
    {
        _orderRepository = orderRepository;
        _customerService = customerService;
        _productService = productService;
    }

    public async Task<IReadOnlyList<OrderResponse>> GetAllAsync(int? customerId, CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetAllAsync(customerId, cancellationToken);
        return orders.Select(OrderResponse.FromEntity).ToList();
    }

    public async Task<OrderResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await GetExistingAsync(id, cancellationToken);
        return OrderResponse.FromEntity(order);
    }

    public async Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        // 1. The customer must exist.
        if (!await _customerService.ExistsAsync(request.CustomerId, cancellationToken))
        {
            throw new BadRequestException($"Customer with id '{request.CustomerId}' does not exist.");
        }

        // 2. Merge duplicate lines, e.g. two lines for product 5 become one line with the summed quantity.
        var lines = request.Items
            .GroupBy(i => i.ProductId)
            .Select(g => new { ProductId = g.Key, Quantity = g.Sum(i => i.Quantity) })
            .ToList();

        // 3. Load the products and make sure they all exist.
        var productIds = lines.Select(l => l.ProductId).ToList();
        var products = (await _productService.GetByIdsAsync(productIds, cancellationToken))
            .ToDictionary(p => p.Id);

        var missingIds = productIds.Where(id => !products.ContainsKey(id)).ToList();
        if (missingIds.Count > 0)
        {
            throw new BadRequestException($"Products not found: {string.Join(", ", missingIds)}.");
        }

        // 4. Build the order using the product's current name and price.
        var order = new Order
        {
            CustomerId = request.CustomerId,
            CreatedAtUtc = DateTime.UtcNow,
            Items = lines.Select(l => new OrderItem
            {
                ProductId = l.ProductId,
                ProductName = products[l.ProductId].Name,
                UnitPrice = products[l.ProductId].Price,
                Quantity = l.Quantity
            }).ToList()
        };
        order.TotalAmount = order.Items.Sum(i => i.UnitPrice * i.Quantity);

        _orderRepository.Add(order);
        await _orderRepository.SaveChangesAsync(cancellationToken);

        return OrderResponse.FromEntity(order);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await GetExistingAsync(id, cancellationToken);

        _orderRepository.Remove(order);
        await _orderRepository.SaveChangesAsync(cancellationToken);
    }

    private async Task<Order> GetExistingAsync(int id, CancellationToken cancellationToken)
        => await _orderRepository.GetByIdAsync(id, cancellationToken)
           ?? throw new NotFoundException(nameof(Order), id);
}
