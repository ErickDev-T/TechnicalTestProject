using Microsoft.EntityFrameworkCore; // si aún no usas EF, no pasa nada si lo dejas
var builder = WebApplication.CreateBuilder(args);
var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔸 Habilitar SIEMPRE para no depender del entorno
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/ping", () => "pong");

// (Opcional por ahora) quita el redirect a HTTPS si te da warning
// app.UseHttpsRedirection();

app.Run();
