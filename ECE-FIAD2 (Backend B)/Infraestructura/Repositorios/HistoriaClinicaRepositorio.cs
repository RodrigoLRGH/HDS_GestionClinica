using Microsoft.EntityFrameworkCore;
using Dominio.Entidades.HistoriasClinicas;
using Aplicacion.Abstracciones;
using Infraestructura.Data;
using System.Linq.Expressions;

namespace Infraestructura.Repositorios
{
    public class HistoriaClinicaRepositorio : RepositorioGenerico<HistoriaClinica>, IHistoriaClinicaRepositorio
    {
        public HistoriaClinicaRepositorio(IDbContextFactory<ContextoECE> factory) : base(factory) { }

        public override async Task<HistoriaClinica?> ObtenerPorIdAsync(int id)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<HistoriaClinica>()
                .Include(h => h.Paciente)
                .FirstOrDefaultAsync(h => h.Id == id && !h.Eliminado);
        }

        public override async Task<IEnumerable<HistoriaClinica>> BuscarAsync(
            Expression<Func<HistoriaClinica, bool>> filtro)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<HistoriaClinica>()
                .Include(h => h.Paciente)
                .Where(h => !h.Eliminado)
                .Where(filtro)
                .ToListAsync();
        }

        public async Task<HistoriaClinica?> ObtenerPorPacienteAsync(int idPaciente)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<HistoriaClinica>()
                .FirstOrDefaultAsync(h => h.IdPaciente == idPaciente && !h.Eliminado);
        }

        public async Task<IEnumerable<HistoriaClinica>> ObtenerConPacienteAsync()
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<HistoriaClinica>()
                .Include(h => h.Paciente)
                .Where(h => !h.Eliminado)
                .ToListAsync();
        }
    }
}