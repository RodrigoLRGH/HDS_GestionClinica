using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.Helpers;

namespace Aplicacion.Servicios.Interfaces
{
    public interface IHistoriaClinicaService
    {
        Task<ResultadoAccion<HistoriaClinicaDTO>> ObtenerPorIdAsync(int id);
        Task<ResultadoAccion<IEnumerable<HistoriaClinicaDTO>>> ObtenerTodosAsync();
        Task<ResultadoAccion<HistoriaClinicaDTO>> CrearAsync(CrearHistoriaClinicaDTO dto);
        Task<ResultadoAccion<HistoriaClinicaDTO>> ActualizarAsync(ActualizarHistoriaClinicaDTO dto);
        Task<ResultadoAccion> EliminarAsync(int id);
        Task<bool> ExisteHistoriaActivaPorPacienteAsync(int idPaciente, int? idExcluir = null);
    }
}