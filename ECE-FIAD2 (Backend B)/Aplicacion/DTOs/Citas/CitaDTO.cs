using Dominio.Enumeraciones;

namespace Aplicacion.DTOs.Citas
{
    public class CitaDTO
    {
        public int Id { get; set; }
        public int IdPaciente { get; set; }
        public string NombrePaciente { get; set; } = string.Empty;
        public int IdDoctor { get; set; }
        public string NombreDoctor { get; set; } = string.Empty;
        public string NombreEspecialidad { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string? Notas { get; set; }
        public EstadoCita Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        public bool Eliminado { get; set; }
    }
}