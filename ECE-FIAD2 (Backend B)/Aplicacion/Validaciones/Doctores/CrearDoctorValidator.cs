using FluentValidation;
using Aplicacion.DTOs.Doctores;

namespace Aplicacion.Validaciones.Doctores
{
    public class CrearDoctorValidator : AbstractValidator<CrearDoctorDTO>
    {
        public CrearDoctorValidator()
        {
            RuleFor(d => d.Nombres)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres");

            RuleFor(d => d.Apellidos)
                .NotEmpty().WithMessage("Los apellidos son obligatorios")
                .MaximumLength(100).WithMessage("Los apellidos no pueden superar 100 caracteres");

            RuleFor(d => d.Email)
                .NotEmpty().WithMessage("El email es obligatorio")
                .EmailAddress().WithMessage("Formato de email incorrecto")
                .MaximumLength(150).WithMessage("El email no puede superar 150 caracteres");

            RuleFor(d => d.Telefono)
                .MaximumLength(15).WithMessage("El teléfono no puede superar 15 caracteres");

            RuleFor(d => d.FechaContratacion)
                .NotEmpty().WithMessage("La fecha de contratación es obligatoria")
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("La fecha de contratación no puede ser futura");

            RuleFor(d => d.IdEspecialidad)
                .GreaterThan(0).WithMessage("Debe seleccionar una especialidad");

            RuleFor(d => d.HorarioAtencion)
                .MaximumLength(200).WithMessage("El horario no puede superar 200 caracteres");
        }
    }
}