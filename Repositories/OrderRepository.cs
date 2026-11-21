using Dot_Net_Basic.Data;
using Dot_Net_Basic.Models;
using Dot_Net_Basic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dot_Net_Basic.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllAsync(int? customerId, CancellationToken cancellationToken = default)
    {
        var query = _context.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .AsQueryable();

        if (customerId is not null)
        {
            query = query.Where(o => o.CustomerId == customerId);
        }

        return await query.OrderByDescending(o => o.CreatedAtUtc).ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

    public void Add(Order order) => _context.Orders.Add(order);

    public void Remove(Order order) => _context.Orders.Remove(order);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}
