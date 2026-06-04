using System;

namespace Aplicacion.DTOs.HistoriasClinicas
{
    public class CrearHistoriaClinicaDTO
    {
        public int IdPaciente { get; set; }
        public DateTime FechaApertura { get; set; } = DateTime.Now;
        public string Alergias { get; set; } = string.Empty;
        public string AntecedentesFamiliares { get; set; } = string.Empty;
        public string AntecedentesPersonales { get; set; } = string.Empty;
    }
}