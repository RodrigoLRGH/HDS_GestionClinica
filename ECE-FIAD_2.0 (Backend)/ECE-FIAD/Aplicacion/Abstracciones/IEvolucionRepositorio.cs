using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entidades.Evoluciones;

namespace Aplicacion.Abstracciones
{
    public interface IEvolucionRepositorio : IRepositorioGenerico<Evolucion>
    {
        Task<IEnumerable<Evolucion?>> ObtenerPaginadoAsync(int pagina, int tamanio, string filtro = null);
        Task<IEnumerable<Evolucion>> ObtenerTodosConRelacionesAsync();
        Task<IEnumerable<Evolucion>> ObtenerPorHistoriaAsync(int idHistoria);
        Task<IEnumerable<Evolucion>> ObtenerPorPacienteAsync(int paciente);
    }
}
