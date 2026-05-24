using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dominio.Enumeraciones;
namespace Aplicacion.DTOs.Pacientes
{
    public class PacienteDTO
    {
        public int Id { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public TipoDeDocumento TipoDocumento { get; set; }
        public string? Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public Genero Genero { get; set; }
        public GrupoSanguineo GrupoSanguineo { get; set; }
        public bool Activo { get; set; }
        public string? Direccion { get; set; } = string.Empty;
        public string NombreCompleto => $"{Nombres} {Apellidos}";
    }
}
