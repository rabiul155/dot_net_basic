using Dot_Net_Basic.Helpers;
using Dot_Net_Basic.DTOs;
using Dot_Net_Basic.Models;
using Dot_Net_Basic.Repositories.Interfaces;
using Dot_Net_Basic.Services.Interfaces;

namespace Dot_Net_Basic.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryService _categoryService;

    public ProductService(IProductRepository productRepository, ICategoryService categoryService)
    {
        _productRepository = productRepository;
        _categoryService = categoryService;
    }

    public async Task<IReadOnlyList<ProductResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        return products.Select(ProductResponse.FromEntity).ToList();
    }

    public async Task<IReadOnlyList<ProductResponse>> GetByIdsAsync(IReadOnlyCollection<int> ids, CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetByIdsAsync(ids, cancellationToken);
        return products.Select(ProductResponse.FromEntity).ToList();
    }

    public async Task<ProductResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await GetExistingAsync(id, cancellationToken);
        return ProductResponse.FromEntity(product);
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        await EnsureCategoryExistsAsync(request.CategoryId, cancellationToken);

        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Quantity = request.Quantity,
            CategoryId = request.CategoryId
        };

        _productRepository.Add(product);
        await _productRepository.SaveChangesAsync(cancellationToken);

        return ProductResponse.FromEntity(product);
    }

    public async Task UpdateAsync(int id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await GetExistingAsync(id, cancellationToken);
        await EnsureCategoryExistsAsync(request.CategoryId, cancellationToken);

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Quantity = request.Quantity;
        product.CategoryId = request.CategoryId;

        await _productRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await GetExistingAsync(id, cancellationToken);

        _productRepository.Remove(product);
        await _productRepository.SaveChangesAsync(cancellationToken);
    }

    private async Task<Product> GetExistingAsync(int id, CancellationToken cancellationToken)
        => await _productRepository.GetByIdAsync(id, cancellationToken)
           ?? throw new NotFoundException(nameof(Product), id);

    private async Task EnsureCategoryExistsAsync(int? categoryId, CancellationToken cancellationToken)
    {
        if (categoryId is not null && !await _categoryService.ExistsAsync(categoryId.Value, cancellationToken))
        {
            throw new BadRequestException($"Category with id '{categoryId}' does not exist.");
        }
    }
}
