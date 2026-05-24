using Dominio.Entidades.HistoriasClinicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Abstracciones
{
    public interface IHistoriaClinicaRepositorio : IRepositorioGenerico<HistoriaClinica>
    {
        Task<IEnumerable<HistoriaClinica?>> ObtenerPaginadoAsync(int pagina, int tamanio, string filtro = null);
        Task<IEnumerable<HistoriaClinica>> ObtenerTodosConRelacionesAsync();
        Task<bool> TieneHistoriaActivaAsync(int idPaciente);
    }
}
