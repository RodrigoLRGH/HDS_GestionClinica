using Aplicacion.DTOs.Evoluciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using System.Threading.Tasks;

namespace Aplicacion.Validaciones.Evoluciones
{
    public class ActualizarEvolucionValidator : AbstractValidator<ActualizarEvolucionDTO>
    {
        public ActualizarEvolucionValidator()
        {
            RuleFor(e => e.IdHistoriaClinica)
                .GreaterThan(0).WithMessage("La historia clínica es obligatoria");

            RuleFor(e => e.IdDoctor)
                .GreaterThan(0).WithMessage("El doctor es obligatorio");

            RuleFor(e => e.Fecha)
               .NotEmpty().WithMessage("La fecha es obligatoria")
               .LessThanOrEqualTo(_ => DateTime.UtcNow.AddMinutes(5))
               .WithMessage("La fecha no puede ser futura");

            RuleFor(e => e.Diagnostico)
                .NotEmpty().WithMessage("El diagnóstico es obligatorio")
                .MaximumLength(500);

            RuleFor(e => e.Tratamiento)
                .NotEmpty().WithMessage("El tratamiento es obligatorio")
                .MaximumLength(500);

            RuleFor(e => e.Notas)
                .MaximumLength(1000);
        }
    }
}
