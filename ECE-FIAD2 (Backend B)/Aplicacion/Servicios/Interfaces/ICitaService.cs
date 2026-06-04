using Aplicacion.DTOs.Citas;
using Aplicacion.Helpers;

namespace Aplicacion.Servicios.Interfaces
{
    public interface ICitaService
    {
        Task<ResultadoAccion<IEnumerable<CitaDTO>>> ObtenerTodosAsync();
        Task<ResultadoAccion<CitaDTO>> ObtenerPorIdAsync(int id);
        Task<ResultadoAccion<CitaDTO>> CrearAsync(CrearCitaDTO dto);
        Task<ResultadoAccion<CitaDTO>> ActualizarAsync(ActualizarCitaDTO dto);
        Task<ResultadoAccion> EliminarAsync(int id);
        Task<bool> ExisteConflictoHorarioAsync(int idDoctor, DateTime fechaHora, int? excluirCitaId = null);
    }
}