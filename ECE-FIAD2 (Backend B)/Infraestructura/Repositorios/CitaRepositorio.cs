using Aplicacion.Abstracciones;
using Dominio.Entidades.Citas;
using Dominio.Enumeraciones;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infraestructura.Repositorios
{
    public class CitaRepositorio : RepositorioGenerico<Cita>, ICitaRepositorio
    {
        public CitaRepositorio(IDbContextFactory<ContextoECE> factory) : base(factory) { }

        public override async Task<Cita?> ObtenerPorIdAsync(int id)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<Cita>()
                .Include(c => c.Paciente)
                .Include(c => c.Doctor).ThenInclude(d => d.Especialidad)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public override async Task<IEnumerable<Cita>> BuscarAsync(
            Expression<Func<Cita, bool>> filtro)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<Cita>()
                .AsNoTracking()
                .Include(c => c.Paciente)
                .Include(c => c.Doctor).ThenInclude(d => d.Especialidad)
                .Where(filtro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cita>> BuscarConIncludesAsync(
            Expression<Func<Cita, bool>> filtro)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<Cita>()
                .Include(c => c.Paciente)
                .Include(c => c.Doctor).ThenInclude(d => d.Especialidad)
                .Where(filtro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cita>> ObtenerCitasDelDiaAsync(DateTime fecha)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<Cita>()
                .Include(c => c.Paciente)
                .Include(c => c.Doctor).ThenInclude(d => d.Especialidad)
                .Where(c => c.FechaHora.Date == fecha.Date && !c.Eliminado)
                .ToListAsync();
        }

        public async Task<bool> ExisteConflictoHorarioAsync(
            int idDoctor, DateTime fechaHora, int? idCitaExcluir = null)
        {
            await using var ctx = _factory.CreateDbContext();
            var consulta = ctx.Set<Cita>().Where(c =>
                c.IdDoctor == idDoctor &&
                c.FechaHora == fechaHora &&
                c.Estado != EstadoCita.Cancelada &&
                !c.Eliminado);

            if (idCitaExcluir.HasValue)
                consulta = consulta.Where(c => c.Id != idCitaExcluir.Value);

            return await consulta.AnyAsync();
        }
    }
}