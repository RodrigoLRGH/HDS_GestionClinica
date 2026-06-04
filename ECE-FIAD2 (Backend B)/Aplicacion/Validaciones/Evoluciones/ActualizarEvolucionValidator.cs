using FluentValidation;
using Aplicacion.DTOs.Evoluciones;

namespace Aplicacion.Validaciones.Evoluciones
{
    public class ActualizarEvolucionValidator : AbstractValidator<ActualizarEvolucionDTO>
    {
        public ActualizarEvolucionValidator()
        {
            RuleFor(e => e.Id)
                .GreaterThan(0).WithMessage("ID inválido.");

            RuleFor(e => e.Diagnostico)
                .NotEmpty().WithMessage("El diagnóstico es obligatorio.")
                .MaximumLength(500).WithMessage("El diagnóstico no puede superar 500 caracteres.");

            RuleFor(e => e.Tratamiento)
                .NotEmpty().WithMessage("El tratamiento es obligatorio.")
                .MaximumLength(500).WithMessage("El tratamiento no puede superar 500 caracteres.");

            RuleFor(e => e.Notas)
                .MaximumLength(1000).WithMessage("Las notas no pueden superar 1000 caracteres.");
        
        
        }
    }
}