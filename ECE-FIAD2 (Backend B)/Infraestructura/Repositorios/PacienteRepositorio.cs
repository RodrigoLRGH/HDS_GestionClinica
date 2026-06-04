using Microsoft.EntityFrameworkCore;
using Dominio.Entidades.Pacientes;
using Aplicacion.Abstracciones;
using Infraestructura.Data;

namespace Infraestructura.Repositorios
{
    public class PacienteRepositorio : RepositorioGenerico<Paciente>, IPacienteRepositorio
    {
        public PacienteRepositorio(IDbContextFactory<ContextoECE> factory) : base(factory) { }

        public async Task<Paciente?> ObtenerConHistoriaAsync(int id)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<Paciente>()
                .Include(p => p.HistoriaClinica)
                .FirstOrDefaultAsync(p => p.Id == id && p.Activo && !p.Eliminado);
        }

        public async Task<Paciente?> ObtenerPorDocumentoAsync(string numeroDocumento)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<Paciente>()
                .FirstOrDefaultAsync(p => p.NumeroDocumento == numeroDocumento);
        }

        public async Task<IEnumerable<Paciente?>> ObtenerPaginadoAsync(
            int pagina, int tamanoPagina, string? filtro = null)
        {
            await using var ctx = _factory.CreateDbContext();
            var query = ctx.Set<Paciente>().AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
                query = query.Where(p =>
                    p.Nombres.Contains(filtro) ||
                    p.Apellidos.Contains(filtro) ||
                    p.NumeroDocumento.Contains(filtro));

            return await query
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();
        }
    }
}