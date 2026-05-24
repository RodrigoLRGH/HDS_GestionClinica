 using Aplicacion.DTOs.Citas;
using Aplicacion.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Servicios.Interfaces
{
    public interface ICitaService
    {
        Task<ResultadoAccion<CitaDTO>> ObtenerPorIdAsync(int id);
        Task<ResultadoAccion<IEnumerable<CitaDTO>>> ObtenerTodosAsync();
        Task<ResultadoAccion<CitaDTO>> CrearAsync(CrearCitaDTO dto);
        Task<ResultadoAccion<CitaDTO>> ActualizarAsync(ActualizarCitaDTO dto);
        Task<ResultadoAccion> EliminarAsync(int id);
    }
}
