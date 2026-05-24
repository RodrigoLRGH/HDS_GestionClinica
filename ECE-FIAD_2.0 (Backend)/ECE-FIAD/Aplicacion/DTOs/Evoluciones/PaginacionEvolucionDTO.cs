using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs.Evoluciones
{
    public class PaginacionEvolucionDTO
    {
        public int Pagina { get; set; } = 1;

        public int TamanoPagina { get; set; } = 10;
        public string? Buscar { get; set; }
        public string? OrdenarPor { get; set; } // "nombres", "identificacion", etc.
        public bool Descendente { get; set; } = false;
    }
}
