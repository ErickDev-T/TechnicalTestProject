using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPIStore.Models;

namespace ProjectAPIStore.Controllers
{
    //https://localhost:44351/api/Usuarios/listed

    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly TestDgadbContext _context;

        public UsuariosController(TestDgadbContext context)
        {
            _context = context;
        }

        [HttpGet("listed")]
        public async Task<ActionResult<List<Usuario>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios); //200
        }
    }
}
