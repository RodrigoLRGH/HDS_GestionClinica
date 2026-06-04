using Aplicacion.DTOs.Citas;
using FluentValidation;

public class CrearCitaValidator : AbstractValidator<CrearCitaDTO>
{
    public CrearCitaValidator()
    {
        RuleFor(c => c.IdPaciente)
            .GreaterThan(0).WithMessage("Debe seleccionar un paciente válido.");

        RuleFor(c => c.IdDoctor)
            .GreaterThan(0).WithMessage("Debe seleccionar un doctor válido.");

        RuleFor(c => c.FechaHora)
            .NotEmpty().WithMessage("La fecha y hora de la cita es obligatoria.")
            .Must(fecha => fecha > DateTime.Now.AddMinutes(-5))
            .WithMessage("La fecha y hora de la cita debe ser futura.");

        RuleFor(c => c.Motivo)
            .NotEmpty().WithMessage("El motivo de la cita es obligatorio.")
            .MaximumLength(500).WithMessage("El motivo no puede superar los 500 caracteres.");

        RuleFor(c => c.Notas)
            .MaximumLength(1000).WithMessage("Las notas no pueden superar los 1000 caracteres.")
            .When(c => !string.IsNullOrEmpty(c.Notas));
    }
}