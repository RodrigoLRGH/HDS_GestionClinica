using Dominio.Entidades.HistoriasClinicas;
using System.Linq.Expressions;

namespace Aplicacion.Abstracciones
{
    public interface IHistoriaClinicaRepositorio : IRepositorioGenerico<HistoriaClinica>
    {
        Task<HistoriaClinica?> ObtenerPorPacienteAsync(int idPaciente);
        Task<IEnumerable<HistoriaClinica>> ObtenerConPacienteAsync();

        new Task<IEnumerable<HistoriaClinica>> BuscarAsync(
           Expression<Func<HistoriaClinica, bool>> filtro);
    }
}