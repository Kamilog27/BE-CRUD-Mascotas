using AutoMapper;
using BE_CRUDMascotas.Models;
using BE_CRUDMascotas.Models.DTO;
using BE_CRUDMascotas.Models.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE_CRUDMascotas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MascotaController : ControllerBase
    {
        
        private readonly IMapper _mapper;
        private readonly IMascotaRepository _mascotaRepository;
        public MascotaController(IMapper mapper,IMascotaRepository mascotaRepository)
        {
            _mapper = mapper;
            _mascotaRepository = mascotaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var listMascotas = await _mascotaRepository.GetListMascotas();
                var listMascotasDto = _mapper.Map<IEnumerable<MascotaDTO>>(listMascotas);
                return Ok(listMascotasDto);
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
                var mascota = _mascotaRepository.GetMascota(id);
                if (mascota == null)
                {
                    return NotFound();
                }
                var mascotadto = _mapper.Map<MascotaDTO>(mascota);
                return Ok(mascotadto);
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
                var mascota = await _mascotaRepository.GetMascota(id);
                if (mascota == null)
                {
                    return NotFound();
                }
                await _mascotaRepository.DeleteMascota(mascota);
                return NoContent();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }
        [HttpPost()]
        public async Task<IActionResult> Post(MascotaDTO mascotaDto)
        {
            try
            {
                var mascota = _mapper.Map<Mascota>(mascotaDto);

                mascota.FechaCreacion = DateTime.Now;
                mascota = await _mascotaRepository.AddMascota(mascota);

                var mascotaItemDto = _mapper.Map<MascotaDTO>(mascota); 
                return CreatedAtAction("Get",new {id=mascotaItemDto.Id},mascotaItemDto);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, MascotaDTO mascotaDto)
        {
            try
            {
                var mascota = _mapper.Map<Mascota>(mascotaDto);
                if (id != mascota.Id)
                {
                    return BadRequest();
                }


                var mascotaItem = await _mascotaRepository.GetMascota(id);
                if (mascotaItem == null)
                {
                    return NotFound();
                }
                await _mascotaRepository.UpdateMascota(mascota);
                return NoContent();
               
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }

    }
}
