using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPIStore.Models; 
using ProjectAPIStore.Data;


namespace ProjectAPIStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly TestDgadbContext _context;

        public ProductsController(TestDgadbContext context)
        {
            _context = context;
        }


        //get products
        [HttpGet("listed")]
        public async Task<ActionResult<List<Producto>>> GetProducts()
        {
            var products = await _context.Productos.ToListAsync();
            return Ok(products); //200
        }



        //edit products
        [HttpPost("SaveProduct")]
        public async Task<ActionResult<Producto>> SaveProduct(Producto producto)

        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, producto);
        }



        // update product
        [HttpPut("update/{id}")]
        public async Task<ActionResult<Producto>> UpdateProduct(int id, Producto producto)
        {
            var dbProd = await _context.Productos.FindAsync(id);
            if (dbProd == null) return NotFound(); // 404

            dbProd.Nombre = producto.Nombre;
            dbProd.Descripcion = producto.Descripcion;
            dbProd.Precio = producto.Precio;
            dbProd.Stock = producto.Stock;

            await _context.SaveChangesAsync();
            return Ok(dbProd);
        }

        // delete product
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var prod = await _context.Productos.FindAsync(id);
            if (prod == null) return NotFound(); // 404

            _context.Productos.Remove(prod);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        // search by id
        [HttpGet("search/{id}")]
        public async Task<ActionResult<Producto>> SearchById(int id)
        {
            var prod = await _context.Productos.FindAsync(id);
            if (prod == null) return NotFound(); // 404
            return Ok(prod);
        }










    }


}
