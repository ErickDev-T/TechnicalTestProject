using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPIStore.Models; // Usuario
using ProjectAPIStore.Data;   // TestDgadbContext

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


        //get users
        [HttpGet("listed")]
        public async Task<ActionResult<List<Usuario>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios); //200
        }
        //edit users
        [HttpPost("SaveUser")]
        public async Task<ActionResult<Usuario>> SaveUser(Usuario usuario)
        {

            usuario.FechaCreacion = DateTime.Now;
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, usuario);
        }

        //update user
        [HttpPut("update/{id}")]
        public async Task<ActionResult<Usuario>> UpdateUser(int id, Usuario usuario)
        {
            var updateUser = await _context.Usuarios.FindAsync(id);
            //checking if the user exist 

            if (updateUser == null)
            {
                return NotFound(); //404
            }

            updateUser.Nombres = usuario.Nombres;
            updateUser.Apellidos = usuario.Apellidos;
            updateUser.Correo = usuario.Correo;
            updateUser.Username = usuario.Username;

            await _context.SaveChangesAsync();
            return Ok(updateUser);
        }


        //delete user
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeletedUser(int id)
        {
            var user = await _context.Usuarios.FindAsync(id);
            //checking if the user exist 
            if (user == null)
            {
                return NotFound(); //404
            }

            _context.Usuarios.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //search by id
        [HttpGet("search/{id}")]
        public async Task<ActionResult<Usuario>> searchById(int id)
        {

            var user = await _context.Usuarios.FindAsync(id);

            //checking if the user exist 
            if (user == null)
            {
                return NotFound(); //404
            }

            return Ok(user);
        }


    }
}
