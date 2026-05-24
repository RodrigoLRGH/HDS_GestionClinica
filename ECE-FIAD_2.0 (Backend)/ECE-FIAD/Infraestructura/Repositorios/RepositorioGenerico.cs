using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aplicacion.Abstracciones;
using Dominio.Entidades.Bases;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Infraestructura.Repositorios
{
    public class RepositorioGenerico<T> : IRepositorioGenerico<T> where T : EntidadBase
    {
        protected readonly ContextoECE _contexto;
        protected readonly DbSet<T> _dbSet;
        public RepositorioGenerico(ContextoECE contexto)
        {
            _contexto = contexto;
            _dbSet = contexto.Set<T>();
        }
        // Filtro base para entidades activas y no eliminadas
        protected IQueryable<T> ObtenActivosNoEliminados()
        {
            return _dbSet.Where(e => e.Activo && !e.Eliminado);
        }
        public async Task<T?> ObtenerPorIdAsync(int id)
        {
            return await ObtenActivosNoEliminados()
                .FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<IEnumerable<T>> ObtenerTodosAsync()
        {
            return await ObtenActivosNoEliminados().ToListAsync();
        }
        public async Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> filtro)
        {
            return await ObtenActivosNoEliminados().Where(filtro).ToListAsync();
        }
        public async Task AgregarAsync(T entidad)
        {
            await _dbSet.AddAsync(entidad);
        }
        public void Actualizar(T entidad)
        {
            _dbSet.Update(entidad);
        }
        public void Eliminar(T entidad)
        {
            _dbSet.Remove(entidad);
        }
        public async Task<int> ContarAsync(Expression<Func<T, bool>>? filtro = null)
        {
            var query = ObtenActivosNoEliminados();
            if (filtro != null)
                query = query.Where(filtro);
            return await query.CountAsync();
        }
        public async Task<bool> ExisteAsync(Expression<Func<T, bool>> predicado)
        {
            return await _dbSet.AnyAsync(predicado);
        }
    }
}