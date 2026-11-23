using Dot_Net_Basic.Models;
using Microsoft.EntityFrameworkCore;

namespace Dot_Net_Basic.Data;

/// <summary>Inserts dummy data for local development. Safe to run repeatedly: each table is only seeded when empty.</summary>
public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        // Categories
        if (!await db.Categories.AnyAsync())
        {
            db.Categories.AddRange(
                new Category { Name = "Electronics", Description = "Phones, laptops and gadgets" },
                new Category { Name = "Books", Description = "Fiction, non-fiction and textbooks" },
                new Category { Name = "Clothing", Description = "Men's and women's apparel" },
                new Category { Name = "Home & Kitchen", Description = "Appliances and household items" },
                new Category { Name = "Sports", Description = "Fitness and outdoor equipment" });
            await db.SaveChangesAsync();
        }

        // Products (need category ids, so look them up by name)
        if (!await db.Products.AnyAsync())
        {
            var categories = await db.Categories.ToDictionaryAsync(c => c.Name, c => c.Id);

            int? Cat(string name) => categories.TryGetValue(name, out var id) ? id : null;

            db.Products.AddRange(
                new Product { Name = "Smartphone X200", Description = "6.5\" OLED display, 128GB storage", Price = 599.99m, Quantity = 50, CategoryId = Cat("Electronics") },
                new Product { Name = "Laptop Pro 15", Description = "15\" laptop, 16GB RAM, 512GB SSD", Price = 1299.00m, Quantity = 25, CategoryId = Cat("Electronics") },
                new Product { Name = "Wireless Earbuds", Description = "Noise cancelling, 24h battery", Price = 79.50m, Quantity = 120, CategoryId = Cat("Electronics") },
                new Product { Name = "Clean Code", Description = "A handbook of agile software craftsmanship", Price = 34.95m, Quantity = 80, CategoryId = Cat("Books") },
                new Product { Name = "The Pragmatic Programmer", Description = "Your journey to mastery", Price = 39.99m, Quantity = 60, CategoryId = Cat("Books") },
                new Product { Name = "Cotton T-Shirt", Description = "100% cotton, unisex", Price = 14.99m, Quantity = 200, CategoryId = Cat("Clothing") },
                new Product { Name = "Denim Jacket", Description = "Classic fit denim jacket", Price = 59.00m, Quantity = 40, CategoryId = Cat("Clothing") },
                new Product { Name = "Coffee Maker", Description = "12-cup programmable coffee maker", Price = 49.99m, Quantity = 35, CategoryId = Cat("Home & Kitchen") },
                new Product { Name = "Non-stick Frying Pan", Description = "28cm non-stick pan", Price = 24.50m, Quantity = 70, CategoryId = Cat("Home & Kitchen") },
                new Product { Name = "Yoga Mat", Description = "6mm thick anti-slip mat", Price = 19.99m, Quantity = 90, CategoryId = Cat("Sports") },
                new Product { Name = "Dumbbell Set 20kg", Description = "Adjustable dumbbell set", Price = 89.00m, Quantity = 30, CategoryId = Cat("Sports") },
                new Product { Name = "Mystery Box", Description = "Uncategorized product", Price = 9.99m, Quantity = 15, CategoryId = null });
            await db.SaveChangesAsync();
        }

        // Customers
        if (!await db.Customers.AnyAsync())
        {
            var now = DateTime.UtcNow;

            db.Customers.AddRange(
                new Customer { FirstName = "John", LastName = "Smith", Email = "john.smith@example.com", Phone = "+1-555-0101", CreatedAtUtc = now.AddDays(-60) },
                new Customer { FirstName = "Emily", LastName = "Johnson", Email = "emily.johnson@example.com", Phone = "+1-555-0102", CreatedAtUtc = now.AddDays(-45) },
                new Customer { FirstName = "Michael", LastName = "Brown", Email = "michael.brown@example.com", Phone = "+1-555-0103", CreatedAtUtc = now.AddDays(-30) },
                new Customer { FirstName = "Sarah", LastName = "Davis", Email = "sarah.davis@example.com", Phone = null, CreatedAtUtc = now.AddDays(-20) },
                new Customer { FirstName = "David", LastName = "Wilson", Email = "david.wilson@example.com", Phone = "+1-555-0105", CreatedAtUtc = now.AddDays(-10) });
            await db.SaveChangesAsync();
        }

        // Orders (need customers and products)
        if (!await db.Orders.AnyAsync())
        {
            var customers = await db.Customers.OrderBy(c => c.Id).ToListAsync();
            var products = await db.Products.ToDictionaryAsync(p => p.Name);

            if (customers.Count == 0 || products.Count == 0)
            {
                return;
            }

            var now = DateTime.UtcNow;

            // (customer index, days ago, [(product name, quantity)])
            var plans = new (int Customer, int DaysAgo, (string Product, int Qty)[] Items)[]
            {
                (0, 25, new[] { ("Smartphone X200", 1), ("Wireless Earbuds", 2) }),
                (1, 18, new[] { ("Clean Code", 1), ("The Pragmatic Programmer", 1) }),
                (2, 12, new[] { ("Cotton T-Shirt", 3), ("Denim Jacket", 1) }),
                (3, 7, new[] { ("Coffee Maker", 1), ("Non-stick Frying Pan", 2) }),
                (4, 3, new[] { ("Yoga Mat", 1), ("Dumbbell Set 20kg", 1) }),
                (0, 1, new[] { ("Laptop Pro 15", 1) }),
            };

            foreach (var plan in plans)
            {
                var items = plan.Items
                    .Where(i => products.ContainsKey(i.Product))
                    .Select(i =>
                    {
                        var product = products[i.Product];
                        return new OrderItem
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            UnitPrice = product.Price,
                            Quantity = i.Qty,
                        };
                    })
                    .ToList();

                if (items.Count == 0)
                {
                    continue;
                }

                db.Orders.Add(new Order
                {
                    CustomerId = customers[plan.Customer % customers.Count].Id,
                    CreatedAtUtc = now.AddDays(-plan.DaysAgo),
                    TotalAmount = items.Sum(i => i.UnitPrice * i.Quantity),
                    Items = items,
                });
            }

            await db.SaveChangesAsync();
        }
    }
}
