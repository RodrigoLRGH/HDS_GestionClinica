using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [ApiController]
    [Route("api/historial")]
    public class HistorialController : ControllerBase
    {
        private readonly IEvolucionService _evolucionService;
        private readonly IHistoriaClinicaService _historiaClinicaService;

        public HistorialController(
            IEvolucionService evolucionService,
            IHistoriaClinicaService historiaClinicaService
        )
        {
            _evolucionService = evolucionService;
            _historiaClinicaService = historiaClinicaService;
        }

        [HttpGet("paciente/{pacienteId}")]
        public async Task<IActionResult> ObtenerPorPaciente(int pacienteId)
        {
            var resultado = await _evolucionService.ObtenerPorPacienteAsync(pacienteId);
            return resultado.Exitoso ? Ok(resultado.Datos) : BadRequest(resultado.Mensaje);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var resultado = await _historiaClinicaService.ObtenerPorIdAsync(id);
            return resultado.Exitoso ? Ok(resultado.Datos) : NotFound(resultado.Mensaje);
        }
    }
}
