namespace Aplicacion.DTOs.Doctores
{
    public class DoctorDTO
    {
        public int Id { get; set; }
        public int IdEspecialidad { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string NombreCompleto => $"{Nombres} {Apellidos}";
        public string NombreEspecialidad { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string HorarioAtencion { get; set; } = string.Empty;
        public DateTime FechaContratacion { get; set; }
        public bool Activo { get; set; }
        public int CantidadCitas { get; set; }
    }
}