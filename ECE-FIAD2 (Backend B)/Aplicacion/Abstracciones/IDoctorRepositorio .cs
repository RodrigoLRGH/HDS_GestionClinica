using Dominio.Entidades.Doctores;

namespace Aplicacion.Abstracciones
{
    public interface IDoctorRepositorio : IRepositorioGenerico<Doctor>
    {
        Task<IEnumerable<Doctor>> ObtenerPorEspecialidadAsync(int idEspecialidad);
        Task<Doctor?> ObtenerConEspecialidadAsync(int id);
        Task<bool> ExisteEmailAsync(string email);
    }
}