using Dot_Net_Basic.Helpers;
using Dot_Net_Basic.DTOs.Categories;
using Dot_Net_Basic.Models;
using Dot_Net_Basic.Repositories.Interfaces;
using Dot_Net_Basic.Services.Interfaces;

namespace Dot_Net_Basic.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IReadOnlyList<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        return categories.Select(CategoryResponse.FromEntity).ToList();
    }

    public async Task<CategoryResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await GetExistingAsync(id, cancellationToken);
        return CategoryResponse.FromEntity(category);
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        => _categoryRepository.ExistsAsync(id, cancellationToken);

    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = new Category
        {
            Name = request.Name,
            Description = request.Description
        };

        _categoryRepository.Add(category);
        await _categoryRepository.SaveChangesAsync(cancellationToken);

        return CategoryResponse.FromEntity(category);
    }

    public async Task UpdateAsync(int id, UpdateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = await GetExistingAsync(id, cancellationToken);

        category.Name = request.Name;
        category.Description = request.Description;

        await _categoryRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await GetExistingAsync(id, cancellationToken);

        _categoryRepository.Remove(category);
        await _categoryRepository.SaveChangesAsync(cancellationToken);
    }

    private async Task<Category> GetExistingAsync(int id, CancellationToken cancellationToken)
        => await _categoryRepository.GetByIdAsync(id, cancellationToken)
           ?? throw new NotFoundException(nameof(Category), id);
}
