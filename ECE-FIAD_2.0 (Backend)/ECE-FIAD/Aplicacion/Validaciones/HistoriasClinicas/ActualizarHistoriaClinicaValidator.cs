using Aplicacion.DTOs.HistoriasClinicas;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Validaciones.HistoriasClinicas
{
    public class ActualizarHistoriaClinicaValidator : AbstractValidator<ActualizarHistoriaClinicaDTO>
    {
        public ActualizarHistoriaClinicaValidator()
        {
            RuleFor(h => h.IdPaciente)
                .GreaterThan(0).WithMessage("El paciente es obligatorio");

            RuleFor(h => h.FechaApertura)
                .NotEmpty().WithMessage("La fecha de apertura es obligatoria")
                .LessThanOrEqualTo(_ => DateTime.UtcNow.AddMinutes(5))
                .WithMessage("La fecha no puede ser futura");

            RuleFor(h => h.Alergias)
                .MaximumLength(500);

            RuleFor(h => h.AntecedentesFamiliares)
                .MaximumLength(1000);

            RuleFor(h => h.AntecedentesPersonales)
                .MaximumLength(1000);
        }

    }
}
