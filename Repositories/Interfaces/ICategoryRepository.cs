using Dot_Net_Basic.Models;

namespace Dot_Net_Basic.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    void Add(Category category);
    void Remove(Category category);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
