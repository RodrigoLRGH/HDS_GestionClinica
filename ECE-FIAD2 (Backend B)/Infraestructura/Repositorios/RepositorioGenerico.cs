using Aplicacion.Abstracciones;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infraestructura.Repositorios
{

    public class RepositorioGenerico<T> : IRepositorioGenerico<T> where T : class
    {
        protected readonly IDbContextFactory<ContextoECE> _factory;

        public RepositorioGenerico(IDbContextFactory<ContextoECE> factory)
        {
            _factory = factory;
        }


        public virtual async Task<IEnumerable<T>> ObtenerTodosAsync()
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<T>().ToListAsync();
        }

        public virtual async Task<T?> ObtenerPorIdAsync(int id)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> BuscarAsync(
            Expression<Func<T, bool>> filtro)
        {
             await using var ctx = _factory.CreateDbContext();
    return await ctx.Set<T>()
        .AsNoTracking()
        .Where(filtro)
        .ToListAsync();
        }

        public virtual async Task<bool> ExisteAsync(
            Expression<Func<T, bool>> filtro)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<T>().AnyAsync(filtro);
        }

        public virtual async Task<int> ContarAsync(Expression<Func<T, bool>>? filtro = null)
        {
            await using var ctx = _factory.CreateDbContext();
            var query = ctx.Set<T>().AsQueryable();
            if (filtro != null)
                query = query.Where(filtro);
            return await query.CountAsync();
        }


        public virtual async Task<T> AgregarAsync(T entidad)
        {
            await using var ctx = _factory.CreateDbContext();
            var entry = await ctx.Set<T>().AddAsync(entidad);
            await ctx.SaveChangesAsync();
            return entry.Entity;
        }


        public virtual async Task Actualizar(T entidad)
        {
            await using var ctx = _factory.CreateDbContext();
            ctx.Set<T>().Update(entidad);
            await ctx.SaveChangesAsync();
        }

        public virtual Task ActualizarAsync(T entidad) => Actualizar(entidad);

        public virtual async Task EliminarAsync(T entidad)
        {
            await using var ctx = _factory.CreateDbContext();
            ctx.Set<T>().Remove(entidad);
            await ctx.SaveChangesAsync();
        }
    }
}