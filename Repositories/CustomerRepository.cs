using Dot_Net_Basic.Data;
using Dot_Net_Basic.Models;
using Microsoft.EntityFrameworkCore;

namespace Dot_Net_Basic.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Customers.AsNoTracking().OrderBy(c => c.Id).ToListAsync(cancellationToken);

    public async Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Customers.AnyAsync(c => c.Id == id, cancellationToken);

    public void Add(Customer customer) => _context.Customers.Add(customer);

    public void Remove(Customer customer) => _context.Customers.Remove(customer);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}
