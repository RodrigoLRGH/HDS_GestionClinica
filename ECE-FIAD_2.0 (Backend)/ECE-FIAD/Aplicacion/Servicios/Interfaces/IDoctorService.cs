using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.Doctores;
using Aplicacion.Helpers;

namespace Aplicacion.Servicios.Interfaces
{
    public interface IDoctorService
    {
        Task<ResultadoAccion<DoctorDTO>> ObtenerPorIdAsync(int id);
        Task<ResultadoAccion<IEnumerable<DoctorDTO>>> ObtenerTodosAsync();
        Task<ResultadoAccion<DoctorDTO>> CrearAsync(CrearDoctorDTO dto);
        Task<ResultadoAccion<DoctorDTO>> ActualizarAsync(ActualizarDoctorDTO dto);
        Task<ResultadoAccion> EliminarAsync(int id);
        Task<ResultadoAccion<IEnumerable<DoctorDTO>>> ObtenerTodosConEspecialidadAsync();
    }
}
