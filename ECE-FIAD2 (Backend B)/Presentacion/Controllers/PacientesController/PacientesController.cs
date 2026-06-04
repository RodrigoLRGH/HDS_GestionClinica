using Aplicacion.DTOs.Pacientes;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [ApiController]
    [Route("api/pacientes")]
    public class PacientesController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;

        public PacientesController(IPacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var resultado = await _pacienteService.ObtenerTodosAsync();
            return resultado.Exitoso ? Ok(resultado.Datos) : BadRequest(resultado.Mensaje);
        }

        [HttpGet("activos")]
        public async Task<IActionResult> ObtenerActivos()
        {
            var resultado = await _pacienteService.ObtenerTodosAsync();
            if (!resultado.Exitoso)
                return BadRequest(resultado.Mensaje);
            var activos = resultado.Datos?.Where(p => p.Activo);
            return Ok(activos);
        }
    }
}
