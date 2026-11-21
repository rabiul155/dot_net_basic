using Dot_Net_Basic.Data;
using Dot_Net_Basic.Models;
using Microsoft.EntityFrameworkCore;

namespace Dot_Net_Basic.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync(cancellationToken);

    public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Categories.AnyAsync(c => c.Id == id, cancellationToken);

    public void Add(Category category) => _context.Categories.Add(category);

    public void Remove(Category category) => _context.Categories.Remove(category);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}
