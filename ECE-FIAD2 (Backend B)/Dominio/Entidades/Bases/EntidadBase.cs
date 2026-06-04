using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades.Bases
{
    public class EntidadBase
    {
        public int Id { get; set; }
        public DateTime FechaDeCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaDeModificacion { get; set; }
        public bool Activo { get; set; } = true;
        public bool Eliminado { get; set; } = false;
        public DateTime? FechaDeEliminacion { get; set; }
    }
}
