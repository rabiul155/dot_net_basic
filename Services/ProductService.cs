using Dot_Net_Basic.Models;
using Dot_Net_Basic.Repositories;

namespace Dot_Net_Basic.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _productRepository.GetAllAsync(cancellationToken);

    public async Task<Product?> GetAsync(int id, CancellationToken cancellationToken = default)
        => await _productRepository.GetByIdAsync(id, cancellationToken);

    public async Task<Product> AddAsync(ProductCreateDto dto, CancellationToken cancellationToken = default)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Quantity = dto.Quantity
        };

        return await _productRepository.AddAsync(product, cancellationToken);
    }

    public async Task<bool> UpdateAsync(int id, ProductUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _productRepository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        var product = new Product
        {
            Id = existing.Id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Quantity = dto.Quantity
        };

        return await _productRepository.UpdateAsync(product, cancellationToken);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        => await _productRepository.DeleteAsync(id, cancellationToken);
}
