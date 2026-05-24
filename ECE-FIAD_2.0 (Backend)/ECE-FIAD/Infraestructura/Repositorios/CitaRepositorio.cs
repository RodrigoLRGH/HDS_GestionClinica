using Aplicacion.Abstracciones;
using Aplicacion.DTOs.Citas;
using Aplicacion.Helpers;
using Dominio.Entidades.Citas;
using Dominio.Enumeraciones;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repositorios
{
    public class CitaRepositorio : RepositorioGenerico<Cita>, ICitaRepositorio
    {
        public CitaRepositorio(ContextoECE contexto) : base(contexto)
        {
        }

        public async Task<IEnumerable<Cita?>> ObtenerPaginadoAsync(int pagina, int tamanioPagina, string? filtro = null)
        {
            var query = _dbSet.AsQueryable();
            if (!string.IsNullOrEmpty(filtro))
            {
                query = query.Where(p => p.Motivo.Contains(filtro));
            }

            return await query.Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cita>> ObtenerTodosConRelacionesAsync()
        {
            return await _contexto.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Doctor)
                .Where(c => !c.Eliminado)
                .ToListAsync();
        }

        public async Task<bool> ExisteDisponibilidadAsync(int idDoctor, DateTime fechaHora, int? excluirCitaId)
        {
            var margen = TimeSpan.FromMinutes(60);
            var query = _contexto.Citas
                .Where(c => c.IdDoctor == idDoctor
                         && !c.Eliminado
                         && c.Estado != EstadoCita.Cancelada
                         && c.FechaHora >= fechaHora.Subtract(margen)
                         && c.FechaHora <= fechaHora.Add(margen));
            if (excluirCitaId.HasValue)
                query = query.Where(c => c.Id != excluirCitaId.Value);

            return await query.AnyAsync();

        }
    }
}
