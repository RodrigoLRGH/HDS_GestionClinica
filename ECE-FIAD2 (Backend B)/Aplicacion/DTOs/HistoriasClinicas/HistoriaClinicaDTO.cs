using System;

namespace Aplicacion.DTOs.HistoriasClinicas
{
    public class HistoriaClinicaDTO
    {
        public int Id { get; set; }
        public int IdPaciente { get; set; }
        public string PacienteNombre { get; set; } = string.Empty;
        public DateTime FechaApertura { get; set; }
        public string Alergias { get; set; } = string.Empty;
        public string AntecedentesFamiliares { get; set; } = string.Empty;
        public string AntecedentesPersonales { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public bool Eliminado { get; set; }
        public DateTime FechaDeCreacion { get; set; }
        public DateTime? FechaDeModificacion { get; set; }
        public DateTime? FechaDeEliminacion { get; set; }
    }
}