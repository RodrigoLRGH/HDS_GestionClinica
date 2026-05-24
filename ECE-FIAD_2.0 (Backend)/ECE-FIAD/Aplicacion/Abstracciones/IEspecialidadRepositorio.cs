using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entidades.Especialidades;

namespace Aplicacion.Abstracciones
{
    public interface IEspecialidadRepositorio : IRepositorioGenerico<Especialidad>
    {
        Task<IEnumerable<Especialidad?>> ObtenerPaginadoAsync(int pagina, int tamanioPagina, string filtro = null);
    }
}
