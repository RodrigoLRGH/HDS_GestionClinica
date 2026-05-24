using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Aplicacion.DTOs.Pacientes;
namespace Aplicacion.Validaciones.Pacientes
{
    public class ActualizarPacienteValidacion : AbstractValidator<ActualizarPacienteDTO>
    {
        public ActualizarPacienteValidacion()
        {
            RuleFor(p => p.Id).GreaterThan(0).WithMessage("El Id debe ser mayor a 0");
            RuleFor(p => p.Nombres)
            .NotEmpty().WithMessage("El nombre es obligatorio")
            .MaximumLength(100).WithMessage("El nombre no pueden superar 100 caracteres");
            RuleFor(p => p.Apellidos)
            .NotEmpty().WithMessage("Los apellidos son obligatorios")
            .MaximumLength(100);
            RuleFor(p => p.Email)
            .EmailAddress().WithMessage("Formato de email incorrecto")
            .MaximumLength(150);
        }
    }
}
