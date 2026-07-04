using Dot_Net_Basic.Models;
using Microsoft.EntityFrameworkCore;

namespace Dot_Net_Basic.Data;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
}
