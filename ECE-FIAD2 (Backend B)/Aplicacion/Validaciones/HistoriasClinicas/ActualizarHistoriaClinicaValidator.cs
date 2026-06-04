using FluentValidation;
using Aplicacion.DTOs.HistoriasClinicas;

namespace Aplicacion.Validaciones.HistoriasClinicas
{
    public class ActualizarHistoriaClinicaValidator : AbstractValidator<ActualizarHistoriaClinicaDTO>
    {
        public ActualizarHistoriaClinicaValidator()
        {
            RuleFor(h => h.Id)
                .GreaterThan(0).WithMessage("ID inválido.");

            RuleFor(h => h.Alergias)
                .MaximumLength(500).WithMessage("Alergias no puede superar los 500 caracteres.");

            RuleFor(h => h.AntecedentesFamiliares)
                .MaximumLength(500).WithMessage("Antecedentes familiares no pueden superar 500 caracteres.");

            RuleFor(h => h.AntecedentesPersonales)
                .MaximumLength(500).WithMessage("Antecedentes personales no pueden superar 500 caracteres.");
        }
    }
}