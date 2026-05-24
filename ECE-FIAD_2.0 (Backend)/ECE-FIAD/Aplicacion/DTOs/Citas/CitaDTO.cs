using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.Pacientes;
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
    public class CitaDTO
    {
        public int Id { get; set; }
        public int IdPaciente {  get; set; }
        public int IdDoctor { get; set; }
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public EstadoCita Estado { get; set; } = EstadoCita.Pendiente;
        public string Notas { get; set; } = string.Empty;
        public virtual PacienteDTO Paciente { get; set; } = null!;
        public virtual DoctorDTO Doctor { get; set; } = null!;
    }
}
