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
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("El Id debe ser mayor a 0");

            RuleFor(p => p.Nombres)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres");

            RuleFor(p => p.Apellidos)
                .NotEmpty().WithMessage("Los apellidos son obligatorios")
                .MaximumLength(100).WithMessage("Los apellidos no pueden superar 100 caracteres");

            RuleFor(p => p.Telefono)
                .MaximumLength(15).WithMessage("El teléfono no puede superar 15 caracteres");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("El email es obligatorio")
                .EmailAddress().WithMessage("Formato de email incorrecto")
                .MaximumLength(100).WithMessage("El email no puede superar 100 caracteres");

            RuleFor(p => p.Direccion)
                .MaximumLength(200).WithMessage("La dirección no puede superar 200 caracteres");
        }
    }
}