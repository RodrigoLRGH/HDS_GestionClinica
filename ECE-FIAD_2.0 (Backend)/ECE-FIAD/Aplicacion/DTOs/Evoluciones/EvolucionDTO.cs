using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entidades.Doctores;
using Dominio.Entidades.Pacientes;
using Dominio.Entidades.Especialidades;
using Dominio.Entidades.HistoriasClinicas;

namespace Aplicacion.DTOs.Evoluciones
{
    public class EvolucionDTO
    {
        public int Id { get; set; }
        public int IdHistoriaClinica { get; set; }
        public int IdDoctor { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string Diagnostico { get; set; } = string.Empty;
        public string Tratamiento { get; set; } = string.Empty;
        public string Notas { get; set; } = string.Empty;
        public virtual HistoriaClinica HistoriaClinica { get; set; } = null!;
        public virtual Doctor Doctor { get; set; } = null!;
        public virtual Especialidad Especialidad { get; set; } = null!;
        public virtual Paciente Paciente { get; set; } = null!;
        public bool Activo { get; set; } = true;
        public DateTime FechaDeCreacion { get; set; } = DateTime.MinValue;
        public DateTime? FechaDeModificacion { get; set; } = DateTime.MinValue;
        public DateTime? FechaDeEliminacion { get; set; } = DateTime.MinValue;
    }
}
