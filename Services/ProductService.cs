using Dot_Net_Basic.Models;

namespace Dot_Net_Basic.Services;

public class ProductService
{
    private readonly List<Product> _products = new();
    private int _nextId = 1;

    public ProductService()
    {
        _products.Add(new Product
        {
            Id = _nextId++,
            Name = "Sample Product",
            Description = "A starter item for the product catalog.",
            Price = 9.99m,
            Quantity = 10
        });
    }

    public IEnumerable<Product> GetAll() => _products;

    public Product? Get(int id) => _products.FirstOrDefault(p => p.Id == id);

    public Product Add(ProductCreateDto dto)
    {
        var product = new Product
        {
            Id = _nextId++,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Quantity = dto.Quantity
        };

        _products.Add(product);
        return product;
    }

    public bool Update(int id, ProductUpdateDto dto)
    {
        var existing = Get(id);
        if (existing is null)
        {
            return false;
        }

        existing.Name = dto.Name;
        existing.Description = dto.Description;
        existing.Price = dto.Price;
        existing.Quantity = dto.Quantity;
        return true;
    }

    public bool Delete(int id)
    {
        return _products.RemoveAll(p => p.Id == id) > 0;
    }
}
