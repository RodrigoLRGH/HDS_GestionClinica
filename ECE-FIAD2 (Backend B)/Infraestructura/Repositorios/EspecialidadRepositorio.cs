using Microsoft.EntityFrameworkCore;
using Dominio.Entidades.Especialidades;
using Aplicacion.Abstracciones;
using Infraestructura.Data;

namespace Infraestructura.Repositorios
{
    public class EspecialidadRepositorio : RepositorioGenerico<Especialidad>, IEspecialidadRepositorio
    {
        public EspecialidadRepositorio(IDbContextFactory<ContextoECE> factory) : base(factory) { }

        public async Task<int> ContarDoctoresPorEspecialidadAsync(int idEspecialidad)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Doctores
                .Where(d => d.IdEspecialidad == idEspecialidad && !d.Eliminado)
                .CountAsync();
        }

        public async Task<bool> TieneDoctoresAsync(int idEspecialidad)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Doctores
                .AnyAsync(d => d.IdEspecialidad == idEspecialidad && !d.Eliminado);
        }

        public async Task<Especialidad?> ObtenerPorNombreAsync(string nombre)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Especialidades
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(e => e.Nombre.ToLower() == nombre.ToLower());
        }

        public async Task<Dictionary<int, int>> ObtenerConteosDoctoresAsync()
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Doctores
                .Where(d => !d.Eliminado)
                .GroupBy(d => d.IdEspecialidad)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Key, x => x.Count);
        }
    }
}