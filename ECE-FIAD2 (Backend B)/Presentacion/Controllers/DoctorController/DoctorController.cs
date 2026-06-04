using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [ApiController]
    [Route("api/doctores")]
    public class DoctoresController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctoresController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var resultado = await _doctorService.ObtenerTodosAsync();
            return resultado.Exitoso ? Ok(resultado.Datos) : BadRequest(resultado.Mensaje);
        }
    }
}
