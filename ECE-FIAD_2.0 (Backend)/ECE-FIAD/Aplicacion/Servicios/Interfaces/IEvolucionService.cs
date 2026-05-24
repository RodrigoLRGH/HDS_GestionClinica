using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.Evoluciones;
using Aplicacion.Helpers;

namespace Aplicacion.Servicios.Interfaces
{
    public interface IEvolucionService
    {
        Task<ResultadoAccion<EvolucionDTO>> ObtenerPorIdAsync(int id);
        Task<ResultadoAccion<IEnumerable<EvolucionDTO>>> ObtenerTodosAsync();
        Task<ResultadoAccion<EvolucionDTO>> CrearAsync(CrearEvolucionDTO dto);
        Task<ResultadoAccion<EvolucionDTO>> ActualizarAsync(ActualizarEvolucionDTO dto);
        Task<ResultadoAccion> EliminarAsync(int id);
        Task<ResultadoAccion<IEnumerable<EvolucionDTO>>> ObtenerPorHistoriaAsync(int idHistoria);
        Task<ResultadoAccion<IEnumerable<EvolucionDTO>>> ObtenerPorPacienteAsync(int pacienteId);

    }
}
