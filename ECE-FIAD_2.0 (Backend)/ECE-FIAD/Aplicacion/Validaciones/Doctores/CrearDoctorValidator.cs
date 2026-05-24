using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.Especialidades;

namespace Aplicacion.Validaciones.Doctores
{
    public class CrearDoctorValidator : AbstractValidator<CrearDoctorDTO>
    {
        public CrearDoctorValidator()
        {
            RuleFor(p => p.Nombres)
            .NotEmpty().WithMessage("Los nombres son obligatorios")
            .MaximumLength(100).WithMessage("Los nombres no puede exceder los 100 caracteres");
            RuleFor(p => p.Apellidos)
            .NotEmpty().WithMessage("Los apellidos son obligatorios")
            .MaximumLength(100);
            RuleFor(p => p.Email)
            .EmailAddress().WithMessage("Formato de email incorrecto")
            .MaximumLength(150);
            RuleFor(p => p.Telefono)
            .MaximumLength(15);
            RuleFor(p => p.Descripcion)
                .MaximumLength(500)
                .WithMessage("La descripcion no puede tener mas de 500 caracteres"); ;
            RuleFor(p => p.FechaContratacion)
                .Must(fecha => fecha.Date <= DateTime.Today)
                .WithMessage("La fecha de contratación no puede ser futura.");
            RuleFor(p => p.IdEspecialidad)
                .GreaterThan(0);

        }
    }
}
