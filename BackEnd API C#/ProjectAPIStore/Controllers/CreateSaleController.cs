using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPIStore.Models;
using ProjectAPIStore.Data;

namespace ProjectAPIStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreateSaleController : ControllerBase
    {
        private readonly TestDgadbContext _context;

        public CreateSaleController(TestDgadbContext context)
        {
            _context = context;
        }

        [HttpPost("ventas")]
        public async Task<IActionResult> CrearVenta([FromBody] CrearVentaDto dto)
        {
            if (dto == null || dto.Items == null || !dto.Items.Any())
                return BadRequest("No se enviaron productos.");

            using var tx = await _context.Database.BeginTransactionAsync();

            // Traer productos involucrados
            var ids = dto.Items.Select(i => i.ProductoId).Distinct().ToList();
            var productos = await _context.Product
                .Where(p => ids.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            // Validar stock
            foreach (var item in dto.Items)
            {
                if (!productos.ContainsKey(item.ProductoId))
                    return BadRequest($"Producto ID {item.ProductoId} no existe.");

                if (productos[item.ProductoId].Stock < item.Cantidad)
                    return BadRequest($"No hay stock suficiente para {productos[item.ProductoId].Nombre}.");
            }

            // Crear venta
            var venta = new Sales
            {
                Cliente = dto.Cliente,
                Fecha = DateTime.Now,
                ListaProductos = string.Join(", ", dto.Items.Select(i => $"{productos[i.ProductoId].Nombre} x{i.Cantidad}")),
                Total = dto.Items.Sum(i => i.Cantidad * productos[i.ProductoId].Precio)
            };

            _context.Sales.Add(venta);
            await _context.SaveChangesAsync();

            // Actualizar stock
            foreach (var item in dto.Items)
            {
                productos[item.ProductoId].Stock -= item.Cantidad;
            }

            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return Ok(venta);
        }
    }

    public class CrearVentaDto
    {
        public string Cliente { get; set; }
        public List<VentaItemDto> Items { get; set; }
    }

    public class VentaItemDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}
