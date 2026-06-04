using Dominio.Entidades.Citas;

namespace Aplicacion.Abstracciones
{
    public interface ICitaRepositorio : IRepositorioGenerico<Cita>
    {
        Task<IEnumerable<Cita>> ObtenerCitasDelDiaAsync(DateTime fecha);
        Task<bool> ExisteConflictoHorarioAsync(int idDoctor, DateTime fechaHora, int? idCitaExcluir = null);
        Task<IEnumerable<Cita>> BuscarConIncludesAsync(
            System.Linq.Expressions.Expression<Func<Cita, bool>> predicado);
    }
}