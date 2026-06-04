using FluentValidation;
using Aplicacion.DTOs.HistoriasClinicas;

namespace Aplicacion.Validaciones.HistoriasClinicas
{
    public class CrearHistoriaClinicaValidator : AbstractValidator<CrearHistoriaClinicaDTO>
    {
        public CrearHistoriaClinicaValidator()
        {
            RuleFor(h => h.IdPaciente)
                .GreaterThan(0).WithMessage("Debe seleccionar un paciente válido.");

            RuleFor(h => h.FechaApertura)
                .Must(fecha => fecha <= DateTime.Now.AddMinutes(5))
                .WithMessage("La fecha de apertura no puede ser futura.");

            RuleFor(h => h.Alergias)
                .MaximumLength(500).WithMessage("Alergias no puede superar los 500 caracteres.");

            RuleFor(h => h.AntecedentesFamiliares)
                .MaximumLength(500).WithMessage("Antecedentes familiares no pueden superar 500 caracteres.");

            RuleFor(h => h.AntecedentesPersonales)
                .MaximumLength(500).WithMessage("Antecedentes personales no pueden superar 500 caracteres.");
        }
    }
}