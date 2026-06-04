using Dominio.Entidades.Especialidades;

namespace Aplicacion.Abstracciones
{
    public interface IEspecialidadRepositorio : IRepositorioGenerico<Especialidad>
    {
        Task<int> ContarDoctoresPorEspecialidadAsync(int idEspecialidad); // ✅ agregado
        Task<bool> TieneDoctoresAsync(int idEspecialidad);
        Task<Especialidad?> ObtenerPorNombreAsync(string nombre);
        Task<Dictionary<int, int>> ObtenerConteosDoctoresAsync();

    }
}