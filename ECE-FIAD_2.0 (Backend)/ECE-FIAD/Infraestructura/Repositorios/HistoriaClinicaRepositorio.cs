using Aplicacion.Abstracciones;
using Dominio.Entidades.HistoriasClinicas;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repositorios
{
    public class HistoriaClinicaRepositorio : RepositorioGenerico<HistoriaClinica>, IHistoriaClinicaRepositorio
    {
        public HistoriaClinicaRepositorio(ContextoECE contexto) : base(contexto)
        {
        }

        public async Task<IEnumerable<HistoriaClinica?>> ObtenerPaginadoAsync(int pagina, int tamanioPagina, string? filtro = null)
        {
            var query = _dbSet.AsQueryable();
            if (!string.IsNullOrEmpty(filtro))
            {
                query = query.Where(h => h.Alergias.Contains(filtro));
            }

            return await query.Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();
        }

        public async Task<IEnumerable<HistoriaClinica>> ObtenerTodosConRelacionesAsync()
        {
            return await _contexto.HistoriasClinicas
                .Where(h => !h.Eliminado)
                .Include(h => h.Paciente)
                .ToListAsync();
        }

        public async Task<bool> TieneHistoriaActivaAsync(int idPaciente)
        {
            return await _contexto.HistoriasClinicas
                .AnyAsync(h => h.IdPaciente == idPaciente
                && h.Activo
                && !h.Eliminado);
        }
    }
}
