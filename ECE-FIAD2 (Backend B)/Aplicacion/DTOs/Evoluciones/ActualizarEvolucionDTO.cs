using System;

namespace Aplicacion.DTOs.Evoluciones
{
    public class ActualizarEvolucionDTO
    {
        public int Id { get; set; }
        public string Diagnostico { get; set; } = string.Empty;
        public string Tratamiento { get; set; } = string.Empty;
        public string Notas { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.Now;
        public bool Activo { get; set; }
    }
}