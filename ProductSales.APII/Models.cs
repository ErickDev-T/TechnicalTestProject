var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// dtos en memoria (temporal) ======
var products = new List<Product>
{
    new() { Id = 1, Name = "Mouse", Description = "Óptico", Price = 10.50m, Stock = 50 },
    new() { Id = 2, Name = "Teclado", Description = "Mecánico", Price = 49.99m, Stock = 20 }
};

// ====== Endpoints Productos ======
var group = app.MapGroup("/api/products");

group.MapGet("/", () => products);

group.MapGet("/{id:int}", (int id) =>
{
    var p = products.FirstOrDefault(x => x.Id == id);
    return p is null ? Results.NotFound() : Results.Ok(p);
});

group.MapPost("/", (Product input) =>
{
    var nextId = products.Count == 0 ? 1 : products.Max(x => x.Id) + 1;
    var p = new Product
    {
        Id = nextId,
        Name = input.Name,
        Description = input.Description,
        Price = input.Price,
        Stock = input.Stock
    };
    products.Add(p);
    return Results.Created($"/api/products/{p.Id}", p);
});

group.MapPut("/{id:int}", (int id, Product input) =>
{
    var p = products.FirstOrDefault(x => x.Id == id);
    if (p is null) return Results.NotFound();
    p.Name = input.Name; p.Description = input.Description; p.Price = input.Price; p.Stock = input.Stock;
    return Results.NoContent();
});

group.MapDelete("/{id:int}", (int id) =>
{
    var p = products.FirstOrDefault(x => x.Id == id);
    if (p is null) return Results.NotFound();
    products.Remove(p);
    return Results.NoContent();
});

// Ping de prueba
app.MapGet("/ping", () => "pong");

app.Run();

// mover a Models product) ======
public class Product    
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
