using Aplicacion.Abstracciones;
using Dominio.Entidades.Evoluciones;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infraestructura.Repositorios
{
    public class EvolucionRepositorio : RepositorioGenerico<Evolucion>, IEvolucionRepositorio
    {
        public EvolucionRepositorio(IDbContextFactory<ContextoECE> factory) : base(factory) { }

        public override async Task<Evolucion?> ObtenerPorIdAsync(int id)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<Evolucion>()
                .Include(e => e.HistoriaClinica).ThenInclude(h => h.Paciente)
                .Include(e => e.Doctor).ThenInclude(d => d.Especialidad)
                .FirstOrDefaultAsync(e => e.Id == id && !e.Eliminado);
        }

        public override async Task<IEnumerable<Evolucion>> BuscarAsync(
            Expression<Func<Evolucion, bool>> filtro)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<Evolucion>()
                .Include(e => e.HistoriaClinica).ThenInclude(h => h.Paciente)
                .Include(e => e.Doctor).ThenInclude(d => d.Especialidad)
                .Where(e => !e.Eliminado)
                .Where(filtro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Evolucion>> ObtenerPorHistoriaClinicaAsync(int idHistoriaClinica)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<Evolucion>()
                .Include(e => e.HistoriaClinica).ThenInclude(h => h.Paciente)
                .Include(e => e.Doctor).ThenInclude(d => d.Especialidad)
                .Where(e => e.IdHistoriaClinica == idHistoriaClinica && !e.Eliminado)
                .OrderByDescending(e => e.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<Evolucion>> ObtenerPorPacienteAsync(int pacienteId)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<Evolucion>()
                .AsNoTracking()
                .Include(e => e.Doctor)
                .Include(e => e.HistoriaClinica)
                    .ThenInclude(h => h.Paciente)
                .Where(e => e.HistoriaClinica.IdPaciente == pacienteId && !e.Eliminado)
                .OrderByDescending(e => e.Fecha)
                .ToListAsync();
        }
    }
}