using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Dominio.Entidades.Doctores;
using Infraestructura.Data;
using Aplicacion.Abstracciones;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Infraestructura.Repositorios
{
    public class DoctorRepositorio : RepositorioGenerico<Doctor>, IDoctorRepositorio
    {
        public DoctorRepositorio(ContextoECE contexto) : base(contexto) 
        {
        }

        public async Task<IEnumerable<Doctor?>> ObtenerPaginadoAsync(int pagina, int tamanioPagina, string? filtro = null)
        {
            var query = _dbSet.AsQueryable();
            if (!string.IsNullOrEmpty(filtro))
            {
                query = query.Where(p => p.Nombres.Contains(filtro) || p.Apellidos.Contains(filtro));
            }

            return await query.Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> ObtenerTodosConEspecialidadAsync()
        {
            return await _contexto.Doctores
                .Include(d => d.Especialidad)
                .Where(d => d.Activo && !d.Eliminado)
                .ToListAsync();
        }
    }
}
