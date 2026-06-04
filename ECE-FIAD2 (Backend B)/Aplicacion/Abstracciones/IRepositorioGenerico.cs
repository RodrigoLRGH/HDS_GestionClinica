using System.Linq.Expressions;

namespace Aplicacion.Abstracciones
{
    public interface IRepositorioGenerico<T> where T : class
    {
        Task<T?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<T>> ObtenerTodosAsync();
        Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> filtro);
        Task<T> AgregarAsync(T entidad);

        Task Actualizar(T entidad);

        Task ActualizarAsync(T entidad);

        Task EliminarAsync(T entidad);
        Task<int> ContarAsync(Expression<Func<T, bool>>? filtro = null);
    }
}