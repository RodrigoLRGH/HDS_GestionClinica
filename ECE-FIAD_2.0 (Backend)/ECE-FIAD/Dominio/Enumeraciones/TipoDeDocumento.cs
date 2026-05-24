using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Enumeraciones
{
    public enum TipoDeDocumento
    {
        [Display(Name = "Cédula")]
        Cedula = 1,
        Pasaporte = 2,
        Otro = 3
    }
}
