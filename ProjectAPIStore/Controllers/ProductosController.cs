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
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _context.Product.ToListAsync();
            return Ok(products); //200
        }



        //edit products
        [HttpPost("SaveProduct")]
        public async Task<ActionResult<Product>> SaveProduct(Product product)

        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, product);
        }



        // update product
        [HttpPut("update/{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
        {
            var dbProd = await _context.Product.FindAsync(id);
            if (dbProd == null) return NotFound(); // 404

            dbProd.Nombre = product.Nombre;
            dbProd.Descripcion = product.Descripcion;
            dbProd.Precio = product.Precio;
            dbProd.Stock = product.Stock;

            await _context.SaveChangesAsync();
            return Ok(dbProd);
        }

        // delete product
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var prod = await _context.Product.FindAsync(id);
            if (prod == null) return NotFound(); // 404

            _context.Product.Remove(prod);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        // search by id
        [HttpGet("search/{id}")]
        public async Task<ActionResult<Product>> SearchById(int id)
        {
            var prod = await _context.Product.FindAsync(id);
            if (prod == null) return NotFound(); // 404
            return Ok(prod);
        }










    }


}
