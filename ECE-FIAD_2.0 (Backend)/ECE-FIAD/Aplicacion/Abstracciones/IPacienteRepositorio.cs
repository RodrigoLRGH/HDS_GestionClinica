using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entidades.Pacientes;

namespace Aplicacion.Abstracciones
{
    public interface IPacienteRepositorio : IRepositorioGenerico<Paciente>
    {
        Task<Paciente?> ObtenerConHistoriaAsync(int id);
        Task<Paciente?> ObtenerPorDocumentoAsync(string numeroDocumento);
        Task<IEnumerable<Paciente?>> ObtenerPaginadoAsync(int pagina, int tamanioPagina, string filtro = null);
    }
}
