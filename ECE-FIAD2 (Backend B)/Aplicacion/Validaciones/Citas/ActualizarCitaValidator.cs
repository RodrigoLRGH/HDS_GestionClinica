using Aplicacion.DTOs.Citas;
using Dominio.Enumeraciones;
using FluentValidation;

namespace Aplicacion.Validaciones.Citas
{
    public class ActualizarCitaValidator : AbstractValidator<ActualizarCitaDTO>
    {
        public ActualizarCitaValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0).WithMessage("El Id de la cita debe ser mayor a 0.");
            RuleFor(c => c.IdPaciente)
                .GreaterThan(0).WithMessage("Debe seleccionar un paciente válido.");
            RuleFor(c => c.IdDoctor)
                .GreaterThan(0).WithMessage("Debe seleccionar un doctor válido.");
            RuleFor(c => c.FechaHora)
                .NotEmpty().WithMessage("La fecha y hora de la cita es obligatoria.")
                .Must(fecha => fecha > DateTime.Now.AddMinutes(-5))
                .WithMessage("La fecha y hora de la cita debe ser futura.")
                .When(c => c.Estado == EstadoCita.Pendiente ||
                           c.Estado == EstadoCita.Confirmada);
            RuleFor(c => c.Motivo)
                .NotEmpty().WithMessage("El motivo de la cita es obligatorio.")
                .MaximumLength(500).WithMessage("El motivo no puede superar los 500 caracteres.");
            RuleFor(c => c.Notas)
                .MaximumLength(1000).WithMessage("Las notas no pueden superar los 1000 caracteres.")
                .When(c => !string.IsNullOrEmpty(c.Notas));
            RuleFor(c => c.Estado)
                .IsInEnum().WithMessage("El estado de la cita no es válido."); 
        }
    }
}