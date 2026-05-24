using Dominio.Entidades.Citas;
using Dominio.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Abstracciones
{
    public interface ICitaRepositorio : IRepositorioGenerico<Cita>
    {
        Task<IEnumerable<Cita?>> ObtenerPaginadoAsync(int pagina, int tamanio, string filtro = null);
        Task<IEnumerable<Cita>> ObtenerTodosConRelacionesAsync();
        Task<bool> ExisteDisponibilidadAsync(int idDoctor, DateTime fechaHora, int? excluirCitaId);    
       
    }
}
