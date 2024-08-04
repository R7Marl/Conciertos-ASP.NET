using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Entity;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("/concerts")]
    public class ConciertosController : ControllerBase
    {

        private readonly AppDbContext _context;
        private ConciertosService conciertosService;
        public ConciertosController(AppDbContext context, ConciertosService conciertosS)
        {
            _context = context;
            conciertosService = conciertosS;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllConcerts([FromQuery] string city)
        {
            try
            {
                IEnumerable<Conciertos> AllConcerts = await conciertosService.GetAllConciertosByCity(city);
                if(AllConcerts.Any())
                {
                    return Ok(AllConcerts);
                }
                return NotFound("No hay conciertos disponibles en la ciudad especificada");
            }catch (Exception ex) 
            {
                return StatusCode(500, "Internal Server Error: "+ex.Message);
            }
        }

    }
}
