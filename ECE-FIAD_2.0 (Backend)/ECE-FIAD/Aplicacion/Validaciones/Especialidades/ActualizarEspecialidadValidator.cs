using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.Especialidades;
using FluentValidation;

namespace Aplicacion.Validaciones.Especialidades
{
    public class ActualizarEspecialidadValidator : AbstractValidator<ActualizarEspecialidadDTO>
    {
        public ActualizarEspecialidadValidator()
        {
            RuleFor(p => p.Id).GreaterThan(0).WithMessage("El Id debe ser mayor a 0");
            RuleFor(p => p.Nombre)
                .NotEmpty().WithMessage("El nombre de la especialidad es obligatorio")
                .MaximumLength(100).WithMessage("El nombre de la especialidad no puede superar los 100 caracteres ");
            RuleFor(p => p.Descripcion)
                .MaximumLength(500).WithMessage("La descripcion no puede superar los 500 caraacteres");
        }
    }
}
