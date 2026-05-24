using Aplicacion.DTOs.Evoluciones;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [ApiController]
    [Route("api/evoluciones")]
    public class EvolucionesController : ControllerBase
    {
        private readonly IEvolucionService _evolucionService;

        public EvolucionesController(IEvolucionService evolucionService)
        {
            _evolucionService = evolucionService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var resultado = await _evolucionService.ObtenerTodosAsync();
            return resultado.Exitoso ? Ok(resultado.Datos) : BadRequest(resultado.Mensaje);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var resultado = await _evolucionService.ObtenerPorIdAsync(id);
            return resultado.Exitoso ? Ok(resultado.Datos) : NotFound(resultado.Mensaje);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearEvolucionDTO dto)
        {
            var resultado = await _evolucionService.CrearAsync(dto);
            return resultado.Exitoso ? Ok(resultado.Datos) : BadRequest(resultado.Mensaje);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarEvolucionDTO dto)
        {
            dto.Id = id;
            var resultado = await _evolucionService.ActualizarAsync(dto);
            return resultado.Exitoso ? Ok(resultado.Datos) : BadRequest(resultado.Mensaje);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _evolucionService.EliminarAsync(id);
            return resultado.Exitoso ? Ok(resultado.Mensaje) : BadRequest(resultado.Mensaje);
        }
    }
}
