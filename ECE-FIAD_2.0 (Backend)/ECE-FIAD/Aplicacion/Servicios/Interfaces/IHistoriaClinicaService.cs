using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Servicios.Interfaces
{
    public interface IHistoriaClinicaService
    {
        Task<ResultadoAccion<HistoriaClinicaDTO>> ObtenerPorIdAsync(int id);
        Task<ResultadoAccion<IEnumerable<HistoriaClinicaDTO>>> ObtenerTodosAsync();
        Task<ResultadoAccion<HistoriaClinicaDTO>> CrearAsync(CrearHistoriaClinicaDTO dto);
        Task<ResultadoAccion<HistoriaClinicaDTO>> ActualizarAsync(ActualizarHistoriaClinicaDTO dto);
        Task<ResultadoAccion> EliminarAsync(int id);
    }
}
