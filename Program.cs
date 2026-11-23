using Dot_Net_Basic.Helpers;
using Dot_Net_Basic.Middleware;
using Dot_Net_Basic.Repositories;
using Dot_Net_Basic.Repositories.Interfaces;
using Dot_Net_Basic.Services;
using Dot_Net_Basic.Services.Interfaces;

// Creates the app builder: loads configuration (appsettings.json, environment variables,
// command-line args), sets up logging, and gives us the DI container (builder.Services).
var builder = WebApplication.CreateBuilder(args);

// Database
// Registers AppDbContext with PostgreSQL using the "DefaultConnection" connection string
// (see Helpers/DatabaseExtensions.cs).
builder.Services.AddDatabase(builder.Configuration);

// Repositories
// AddScoped = one instance per HTTP request. Whenever a class asks for the interface
// (e.g. IProductRepository), the DI container gives it the concrete class (ProductRepository).
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Services
// Business-logic layer, also one instance per request. Controllers depend on these
// interfaces, and the services in turn use the repositories above.
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Registers the services MVC needs for API controllers (routing to controller actions,
// model binding, JSON serialization, validation). Does not map any routes by itself;
// that happens later with app.MapControllers().
builder.Services.AddControllers();

// Error handling (Middleware/GlobalExceptionHandler.cs)
// AddProblemDetails: errors are returned in the standard RFC 7807 "ProblemDetails" JSON format.
builder.Services.AddProblemDetails();
// Registers our custom handler that catches unhandled exceptions and turns them into responses.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Builds the WebApplication from everything registered above. After this line,
// no more services can be added; from here on we configure the request pipeline.
var app = builder.Build();

// Adds the exception-handling middleware to the pipeline so any unhandled exception
// is passed to GlobalExceptionHandler. Added first so it wraps everything after it.
app.UseExceptionHandler();

// A minimal API endpoint: GET / returns "Hello World!" (handy as a quick health check).
app.MapGet("/", () => "Hello World!");
// Maps routes for all controllers using their [Route] / [HttpGet] etc. attributes.
app.MapControllers();

// Apply pending migrations automatically, but only on your local machine.
// IsDevelopment() is true when ASPNETCORE_ENVIRONMENT=Development.
if (app.Environment.IsDevelopment())
{
    // Runs any EF Core migrations not yet applied to the database (creates/updates tables).
    await app.ApplyMigrationsAsync();
}

// Seed command: `dotnet run -- seed`
// Fills empty tables with dummy data (Data/DbSeeder.cs), then exits without starting the web server.
// Normal `dotnet run` does NOT seed.
if (args.Contains("seed", StringComparer.OrdinalIgnoreCase))
{
    await app.ApplyMigrationsAsync();
    await app.SeedDatabaseAsync();
    Console.WriteLine("Database seeded.");
    return;
}

// Starts the web server and blocks, listening for HTTP requests until the app is shut down.
app.Run();
