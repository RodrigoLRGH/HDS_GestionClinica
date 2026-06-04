using Aplicacion.DTOs.Citas;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [ApiController]
    [Route("api/citas")]
    public class CitasController : ControllerBase
    {
        private readonly ICitaService _citaService;

        public CitasController(ICitaService citaService)
        {
            _citaService = citaService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var resultado = await _citaService.ObtenerTodosAsync();
            return resultado.Exitoso ? Ok(resultado.Datos) : BadRequest(resultado.Mensaje);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var resultado = await _citaService.ObtenerPorIdAsync(id);
            return resultado.Exitoso ? Ok(resultado.Datos) : NotFound(resultado.Mensaje);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearCitaDTO dto)
        {
            var resultado = await _citaService.CrearAsync(dto);
            if (resultado.Exitoso)
                return Ok(resultado.Datos);
            return BadRequest(new { mensaje = resultado.Mensaje, errores = resultado.Errores });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarCitaDTO dto)
        {
            dto.Id = id;
            var resultado = await _citaService.ActualizarAsync(dto);
            return resultado.Exitoso ? Ok(resultado.Datos) : BadRequest(resultado.Mensaje);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _citaService.EliminarAsync(id);
            return resultado.Exitoso ? Ok(resultado.Mensaje) : BadRequest(resultado.Mensaje);
        }
    }
}
