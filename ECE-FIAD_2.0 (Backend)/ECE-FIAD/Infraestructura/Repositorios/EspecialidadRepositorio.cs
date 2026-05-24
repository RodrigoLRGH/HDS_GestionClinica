using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dominio.Entidades.Especialidades;
using Infraestructura.Data;
using Aplicacion.Abstracciones;

using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Infraestructura.Repositorios
{
    public class EspecialidadRepositorio : RepositorioGenerico<Especialidad>, IEspecialidadRepositorio
    {
        public EspecialidadRepositorio(ContextoECE contexto) : base(contexto)
        {
        }
        public async Task<IEnumerable<Especialidad>> ObtenerTodosConMedicosAsync()
        {
            return await _dbSet
                .Include(e => e.Doctores)
                .ToListAsync();
        }

        public async Task<IEnumerable<Especialidad?>> ObtenerPaginadoAsync(int pagina, int tamanioPagina, string? filtro = null)
        {
            var query = _dbSet
                .Include(e => e.Doctores)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
                query = query.Where(p => p.Nombre.Contains(filtro));

            return await query
                .Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();
        }
    }
}
