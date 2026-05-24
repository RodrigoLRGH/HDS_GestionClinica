using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entidades.Citas;
using Dominio.Entidades.Especialidades;

namespace Aplicacion.DTOs.Doctores
{
    public class CrearDoctorDTO
    {
        public int IdEspecialidad { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string HorarioAtencion { get; set; } = string.Empty;
        public virtual Especialidad Especialidad { get; set; } = null!;
        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
        public string NombreCompleto => $"{Nombres} {Apellidos}";
        public DateTime FechaContratacion { get; set; } = DateTime.Now;
        public string Descripcion { get; set; } = string.Empty;
    }
}
