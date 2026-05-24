using Dominio.Entidades.Evoluciones;
using Dominio.Entidades.Pacientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs.HistoriasClinicas
{
    public class ActualizarHistoriaClinicaDTO
    {
        public int Id { get; set; }
        public int IdPaciente { get; set; }
        public DateTime FechaApertura { get; set; } 
        public string Alergias { get; set; } = string.Empty;
        public string AntecedentesFamiliares { get; set; } = string.Empty;
        public string AntecedentesPersonales { get; set; } = string.Empty;

        public virtual Paciente Paciente { get; set; } = null!;
        public virtual ICollection<Evolucion> Evoluciones { get; set; } = new List<Evolucion>();
        public bool Activo { get; set; } = true;
    }
}
