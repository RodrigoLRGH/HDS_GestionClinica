namespace Aplicacion.DTOs.Doctores
{
    public class ActualizarDoctorDTO
    {
        public int Id { get; set; }
        public int IdEspecialidad { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string HorarioAtencion { get; set; } = string.Empty;
        public DateTime FechaContratacion { get; set; }
    }
}