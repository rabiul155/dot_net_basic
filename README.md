# Dot_Net_Basic

An ASP.NET Core (.NET 10) Web API with PostgreSQL, using a classic layered structure.

## Project structure

```
Controllers/                <- HTTP endpoints only (ProductsController, CategoriesController, ...)
DTOs/                       <- request/response classes the API receives and returns
Data/
  AppDbContext.cs           <- EF Core database context (DbSets)
  Configurations/           <- table settings: column lengths, keys, indexes, relations
Helpers/                    <- custom exceptions + database setup helpers
Middleware/                 <- GlobalExceptionHandler: turns exceptions into JSON errors
Migrations/                 <- EF Core migrations
Models/                     <- database entities (Product, Category, Customer, Order, OrderItem)
Repositories/               <- database queries (interface + implementation)
Services/                   <- business logic (interface + implementation)
Program.cs                  <- registers everything and starts the app
Dot_Net_Basic.http          <- example requests for every endpoint
```

A request flows: **Controller -> Service -> Repository -> AppDbContext -> PostgreSQL**.

## Endpoints

| Resource   | Route             | Operations                                                  |
|------------|-------------------|-------------------------------------------------------------|
| Products   | `/api/products`   | GET all, GET by id, POST, PUT, DELETE                       |
| Categories | `/api/categories` | GET all, GET by id, POST, PUT, DELETE                       |
| Customers  | `/api/customers`  | GET all, GET by id, POST, PUT, DELETE                       |
| Orders     | `/api/orders`     | GET all (`?customerId=1` filter), GET by id, POST, DELETE   |

## Running

```bash
dotnet run
```

Pending migrations are applied automatically on startup in the Development environment.

## Adding a new feature (e.g. `Supplier`)

1. `Models/Supplier.cs` - the entity
2. `Data/Configurations/SupplierConfiguration.cs` - table settings, and a `DbSet<Supplier>` in `AppDbContext`
3. `DTOs/` - `CreateSupplierRequest`, `UpdateSupplierRequest`, `SupplierResponse`
4. `Repositories/` - `ISupplierRepository` + `SupplierRepository`
5. `Services/` - `ISupplierService` + `SupplierService`
6. `Controllers/SuppliersController.cs`
7. Register the repository and service in `Program.cs`
8. Create a migration:

```bash
dotnet ef migrations add AddSuppliers
```
