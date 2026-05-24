using Aplicacion.Abstracciones;
using Aplicacion.DTOs.Citas;
using FluentValidation;

namespace Aplicacion.Validaciones.Citas
{
    public class CrearCitaValidator : AbstractValidator<CrearCitaDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CrearCitaValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(c => c.IdPaciente)
            .GreaterThan(0).WithMessage("El paciente es obligatorio");

            RuleFor(c => c.IdDoctor)
                .NotEmpty().WithMessage("El doctor es requerido")
                .GreaterThan(0).WithMessage("El doctor es obligatorio");
                

            RuleFor(c => c.FechaHora)
                .NotEmpty().WithMessage("La fecha y hora son obligatorias")
                .GreaterThan(DateTime.Now).WithMessage("La cita debe ser en el futuro");

            RuleFor(c => c.Motivo)
                .NotEmpty().WithMessage("El motivo es obligatorio")
                .MinimumLength(5).WithMessage("Ingrese un motivo válido")
                .MaximumLength(500);

            RuleFor(c => c.Notas)
                .NotEmpty().WithMessage("Las notas son obligatorias")
                .MinimumLength(5).WithMessage("Ingrese notas válidas")
                .MaximumLength(1000);

            RuleFor(c => c.Estado)
                .IsInEnum().WithMessage("Estado inválido");

        }
    }
}