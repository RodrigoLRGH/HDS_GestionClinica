using Dominio.Entidades.Evoluciones;

namespace Aplicacion.Abstracciones
{
    public interface IEvolucionRepositorio : IRepositorioGenerico<Evolucion>
    {
        Task<IEnumerable<Evolucion>> ObtenerPorHistoriaClinicaAsync(int idHistoriaClinica);
        Task<IEnumerable<Evolucion>> ObtenerPorPacienteAsync(int paciente);

    }
}