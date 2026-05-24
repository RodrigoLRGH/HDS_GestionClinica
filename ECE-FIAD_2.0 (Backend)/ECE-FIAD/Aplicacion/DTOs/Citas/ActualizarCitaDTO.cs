using Dominio.Entidades.Doctores;
using Dominio.Entidades.Pacientes;
using Dominio.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs.Citas
{
    public class ActualizarCitaDTO
    {
        public int Id { get; set; }
        public int IdPaciente { get; set; }
        public int IdDoctor { get; set; }
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public EstadoCita Estado { get; set; } = EstadoCita.Pendiente;
        public string Notas { get; set; } = string.Empty;
        // public virtual Paciente Paciente { get; set; } = null!;
        // public virtual Doctor Doctor { get; set; } = null!;
    }
}
