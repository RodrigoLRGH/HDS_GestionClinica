using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.Doctores;
using Aplicacion.Helpers;
using Dominio.Entidades.Doctores;

namespace Aplicacion.Abstracciones
{
    public interface IDoctorRepositorio : IRepositorioGenerico<Doctor>
    {
        Task<IEnumerable<Doctor?>> ObtenerPaginadoAsync(int pagina, int tamanio, string filtro = null);
        Task<IEnumerable<Doctor>> ObtenerTodosConEspecialidadAsync();
    }
}
