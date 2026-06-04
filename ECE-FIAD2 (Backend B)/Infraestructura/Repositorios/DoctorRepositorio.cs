using Microsoft.EntityFrameworkCore;
using Dominio.Entidades.Doctores;
using Aplicacion.Abstracciones;
using Infraestructura.Data;

namespace Infraestructura.Repositorios
{
    public class DoctorRepositorio : RepositorioGenerico<Doctor>, IDoctorRepositorio
    {
        public DoctorRepositorio(IDbContextFactory<ContextoECE> factory) : base(factory) { }

        public override async Task<IEnumerable<Doctor>> ObtenerTodosAsync()
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<Doctor>()
                .Include(d => d.Especialidad)
                .Include(d => d.Citas)
                .ToListAsync();
        }

        public override async Task<Doctor?> ObtenerPorIdAsync(int id)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<Doctor>()
                .Include(d => d.Especialidad)
                .Include(d => d.Citas)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Doctor>> ObtenerPorEspecialidadAsync(int idEspecialidad)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<Doctor>()
                .Include(d => d.Especialidad)
                .Where(d => d.IdEspecialidad == idEspecialidad)
                .ToListAsync();
        }

        public async Task<Doctor?> ObtenerConEspecialidadAsync(int id)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<Doctor>()
                .Include(d => d.Especialidad)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Doctor?> ObtenerPorEmailAsync(string email)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<Doctor>()
                .FirstOrDefaultAsync(d => d.Email == email);
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            await using var ctx = _factory.CreateDbContext();
            return await ctx.Set<Doctor>()
                .AnyAsync(d => d.Email == email);
        }
    }
}