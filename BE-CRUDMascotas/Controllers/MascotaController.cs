using BE_CRUDMascotas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE_CRUDMascotas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MascotaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MascotaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var listMascotas=await _context.Mascotas.ToListAsync();
                return Ok(listMascotas);
            }
            catch (Exception e)
            {

                return BadRequest (e.Message);
            }
            
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var mascota = await _context.Mascotas.FindAsync(id);
                if (mascota == null)
                {
                    return NotFound();
                }
                return Ok(mascota);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var mascota = await _context.Mascotas.FindAsync(id);
                if (mascota == null)
                {
                    return NotFound();
                }
                _context.Mascotas.Remove(mascota);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }
        [HttpPost()]
        public async Task<IActionResult> Post(Mascota mascota)
        {
            try
            {
                mascota.FechaCreacion = DateTime.Now;
                
                _context.Add(mascota);
                await _context.SaveChangesAsync();
                return CreatedAtAction("Get",new {id=mascota.Id},mascota);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id,Mascota mascota)
        {
            try
            {
                if (id != mascota.Id)
                {
                    return BadRequest();
                }
                

                var mascotaItem=await _context.Mascotas.FindAsync(id);
                if (mascotaItem == null)
                {
                    return NotFound();
                }
                mascotaItem.Nombre= mascota.Nombre;
                mascotaItem.Edad = mascota.Edad;
                mascotaItem.Raza = mascota.Raza;
                mascotaItem.Color = mascota.Color;
                mascotaItem.Peso = mascota.Peso;
                await _context.SaveChangesAsync();
                return NoContent();
               
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }

    }
}
