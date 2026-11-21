using Dot_Net_Basic.Data;
using Dot_Net_Basic.Models;
using Dot_Net_Basic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dot_Net_Basic.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Products.AsNoTracking().OrderBy(p => p.Id).ToListAsync(cancellationToken);

    public async Task<List<Product>> GetByIdsAsync(IReadOnlyCollection<int> ids, CancellationToken cancellationToken = default)
        => await _context.Products.AsNoTracking().Where(p => ids.Contains(p.Id)).ToListAsync(cancellationToken);

    // Tracked (no AsNoTracking) so the service can change it and then call SaveChangesAsync.
    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public void Add(Product product) => _context.Products.Add(product);

    public void Remove(Product product) => _context.Products.Remove(product);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}
