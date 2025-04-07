using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NanoMediatorAspNetSample.Database;
using NanoMediatorAspNetSample.Implementation;
using utils_netcore_cqrs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("ProductsDb"));

builder.Services.AddNanoMediator(typeof(Program).Assembly);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product API with Nano Mediator",
        Version = "v1"
    });
});

var app = builder.Build();

// --- // --- //

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

    db.Products.AddRange(
        new Product
        {
            Name = "Laptop",
            Price = 999.99m
        },
        new Product
        {
            Name = "Mouse",
            Price = 19.99m
        }
        );

    db.SaveChanges();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Custom Mediator API v1");
        c.RoutePrefix = string.Empty; // Makes Swagger available at root URL
    });
}

app.MapGet("/products", async (INanoMediator mediator) =>
{
    var products = await mediator.Send(new AllProductsQuery());
    return Results.Ok(products);
})
.WithName("GetAllProducts")
.WithOpenApi(); ;

app.MapPost("/products", async (INanoMediator mediator, CreateProductCommand command) =>
{
    var product = await mediator.Send(command);
    return Results.Created($"/products/{product.Id}", product);
})
.WithName("CreateProduct")
.WithOpenApi();

app.Run();