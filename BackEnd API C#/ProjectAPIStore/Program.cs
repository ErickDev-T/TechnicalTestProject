using Microsoft.EntityFrameworkCore;
using ProjectAPIStore.Data;  
using ProjectAPIStore.Models; 

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<TestDgadbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connectionDB"))
);

// CORS: permite tu Vite en dev
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:5173") // URL de Vite
              .AllowAnyHeader()
              .AllowAnyMethod()
    );
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// redirigir raíz a Swagger
app.MapGet("/", ctx =>
{
    ctx.Response.Redirect("/swagger/index.html", permanent: false); // direct to that URL
    return Task.CompletedTask;
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseCors("AllowFrontend");  

app.UseAuthorization();

app.MapControllers();

app.Run();
