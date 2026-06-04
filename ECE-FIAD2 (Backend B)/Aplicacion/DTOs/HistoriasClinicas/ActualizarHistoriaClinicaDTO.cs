using System;

namespace Aplicacion.DTOs.HistoriasClinicas
{
    public class ActualizarHistoriaClinicaDTO
    {
        public int Id { get; set; }
        public string Alergias { get; set; } = string.Empty;
        public string AntecedentesFamiliares { get; set; } = string.Empty;
        public string AntecedentesPersonales { get; set; } = string.Empty;
    }
}