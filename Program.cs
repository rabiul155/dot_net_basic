using Dot_Net_Basic.Helpers;
using Dot_Net_Basic.Middleware;
using Dot_Net_Basic.Repositories;
using Dot_Net_Basic.Repositories.Interfaces;
using Dot_Net_Basic.Services;
using Dot_Net_Basic.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDatabase(builder.Configuration);

// Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddControllers();

// Error handling (Middleware/GlobalExceptionHandler.cs)
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

app.MapGet("/", () => "Hello World!");
app.MapControllers();

// Apply pending migrations automatically, but only on your local machine.
if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrationsAsync();
}

app.Run();
