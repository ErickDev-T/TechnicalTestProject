using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPIStore.Data;
using ProjectAPIStore.Models;

namespace ProjectAPIStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // => /api/CreateSale
    public class CreateSaleController : ControllerBase
    {
        private readonly TestDgadbContext _context;
        public CreateSaleController(TestDgadbContext context) => _context = context;


        [HttpPost("ventas")]
        public async Task<IActionResult> CrearVenta([FromBody] CreateSell dto)
        {
            if (dto is null || dto.Items is null || !dto.Items.Any())
                return BadRequest("No se enviaron productos.");

            using var tx = await _context.Database.BeginTransactionAsync();

            // productos involucrados
            var ids = dto.Items.Select(i => i.ProductoId).Distinct().ToList();
            var productos = await _context.Product
                .Where(p => ids.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            // validar
            foreach (var it in dto.Items)
            {
                if (!productos.ContainsKey(it.ProductoId))
                    return BadRequest($"Producto ID {it.ProductoId} no existe.");

                if (it.Cantidad <= 0)
                    return BadRequest($"Cantidad inválida para producto {it.ProductoId}.");

                var p = productos[it.ProductoId];
                if (p.Stock < it.Cantidad)
                    return BadRequest($"Stock insuficiente para {p.Nombre}. Disponible: {p.Stock}");
            }

            // calcular total y lista
            decimal total = 0m;
            var partes = new List<string>();
            foreach (var it in dto.Items)
            {
                var p = productos[it.ProductoId];
                var precio = it.PrecioUnitario ?? p.Precio; // si no viene, usa el de Productos
                total += precio * it.Cantidad;
                partes.Add($"{p.Nombre} x{it.Cantidad}");
            }

            // crear venta
            var venta = new Sales
            {
                Cliente = string.IsNullOrWhiteSpace(dto.Cliente) ? "Consumidor final" : dto.Cliente,
                Fecha = DateTime.Now,
                ListaProductos = string.Join(", ", partes),
                Total = total
            };

            _context.Sales.Add(venta);
            await _context.SaveChangesAsync();

            // descontar stock
            foreach (var it in dto.Items)
            {
                var p = productos[it.ProductoId];
                p.Stock -= it.Cantidad;
            }

            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return Ok(venta);
        }
    }
}
