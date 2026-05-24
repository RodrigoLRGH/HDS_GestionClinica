using Aplicacion.Abstracciones;
using Dominio.Entidades.Evoluciones;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repositorios
{
    public class EvolucionRepositorio : RepositorioGenerico<Evolucion>, IEvolucionRepositorio
    {
        public EvolucionRepositorio(ContextoECE contexto) : base(contexto)
        {
        }

        public async Task<IEnumerable<Evolucion?>> ObtenerPaginadoAsync(int pagina, int tamanioPagina, string? filtro = null)
        {
            var query = _dbSet.AsQueryable();
            if (!string.IsNullOrEmpty(filtro))
            {
                query = query.Where(p => p.Diagnostico.Contains(filtro));
            }

            return await query.Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();
        }

        public async Task<IEnumerable<Evolucion>> ObtenerTodosConRelacionesAsync()
        {
            return await _contexto.Evoluciones
                .Include(e => e.Doctor)
                .Include(e => e.HistoriaClinica)
                    .ThenInclude(h => h.Paciente)
                .Where(e => !e.Eliminado)
                .OrderByDescending(e => e.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<Evolucion>> ObtenerPorHistoriaAsync(int idHistoria)
        {
            return await _contexto.Evoluciones
                .AsNoTracking()
                .Include(e => e.Doctor)
                .Include(e => e.HistoriaClinica)
                    .ThenInclude(h => h.Paciente)
                .Where(e => e.IdHistoriaClinica == idHistoria && !e.Eliminado)
                .OrderByDescending(e => e.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<Evolucion>> ObtenerPorPacienteAsync(int pacienteId)
        {
            return await _contexto.Evoluciones
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
