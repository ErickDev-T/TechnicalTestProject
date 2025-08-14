using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPIStore.Models;  // Venta
using ProjectAPIStore.Data;    // TestDgadbContext

namespace ProjectAPIStore.Controllers
{
    // https://localhost:44351/api/Ventas/listed
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly TestDgadbContext _context;

        public VentasController(TestDgadbContext context)
        {
            _context = context;
        }

        // get ventas
        [HttpGet("listed")]
        public async Task<ActionResult<List<Sales>>> GetVentas()
        {
            var sales = await _context.Sales.ToListAsync();
            return Ok(sales); //200
        }

        // create venta
        [HttpPost("SaveVenta")]
        public async Task<ActionResult<Sales>> SaveVenta(Sales sales)
        {
            if (sales.Fecha == default) sales.Fecha = DateTime.Now;
            _context.Sales.Add(sales);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, sales);
        }

        // update venta
        [HttpPut("update/{id}")]
        public async Task<ActionResult<Sales>> UpdateVenta(int id, Sales sales)
        {
            var updateVenta = await _context.Sales.FindAsync(id);
            if (updateVenta == null)
            {
                return NotFound(); //404
            }

            updateVenta.Fecha = sales.Fecha;
            updateVenta.Cliente = sales.Cliente;
            updateVenta.ListaProductos = sales.ListaProductos;
            updateVenta.Total = sales.Total;

            await _context.SaveChangesAsync();
            return Ok(updateVenta);
        }

        // delete venta
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeletedVenta(int id)
        {
            var v = await _context.Sales.FindAsync(id);
            if (v == null)
            {
                return NotFound(); //404
            }

            _context.Sales.Remove(v);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // search by id
        [HttpGet("search/{id}")]
        public async Task<ActionResult<Sales>> searchById(int id)
        {
            var sales = await _context.Sales.FindAsync(id);
            if (sales == null)
            {
                return NotFound(); //404
            }

            return Ok(sales);
        }





    }
}
