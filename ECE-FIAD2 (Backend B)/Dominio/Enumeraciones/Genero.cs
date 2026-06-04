using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Enumeraciones
{
    public enum Genero
    {
        Masculino = 1,
        Femenino = 2,
        Otro = 3,
        [Display(Name = "Prefiero no decir")]
        PrefieroNoDecir = 4
    }
}
