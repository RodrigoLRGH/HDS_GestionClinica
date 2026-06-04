using Aplicacion.DTOs.Pacientes;
using Aplicacion.Helpers;

namespace Aplicacion.Servicios.Interfaces
{
    public interface IPacienteService
    {
        Task<ResultadoAccion<PacienteDTO>> ObtenerPorIdAsync(int id);
        Task<ResultadoAccion<IEnumerable<PacienteDTO>>> ObtenerTodosAsync();
        Task<ResultadoAccion<PacienteDTO>> CrearAsync(CrearPacienteDTO dto);
        Task<ResultadoAccion<PacienteDTO>> ActualizarAsync(ActualizarPacienteDTO dto);
        Task<ResultadoAccion> EliminarAsync(int id);
        Task<bool> ExistePorDocumentoAsync(string numeroDocumento);
    }
}