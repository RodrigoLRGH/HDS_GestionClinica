using Aplicacion.DTOs.Pacientes;
using FluentValidation;
using System;

namespace Aplicacion.Validaciones.Pacientes
{
    public class CrearPacienteValidator : AbstractValidator<CrearPacienteDTO>
    {
        public CrearPacienteValidator()
        {
            RuleFor(p => p.Nombres)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100);

            RuleFor(p => p.Apellidos)
                .NotEmpty().WithMessage("Los apellidos son obligatorios")
                .MaximumLength(100);

            RuleFor(p => p.NumeroDocumento)
                .NotEmpty().WithMessage("El número de identificación es obligatorio")
                .MaximumLength(20);

            RuleFor(p => p.FechaNacimiento)
                .NotEmpty().WithMessage("La fecha de nacimiento es obligatoria")
                .LessThan(DateTime.Today).WithMessage("La fecha de nacimiento debe ser anterior al día de hoy")
                .Must(fecha => fecha > DateTime.Today.AddYears(-120)).WithMessage("Fecha de nacimiento no válida");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("El email es obligatorio")
                .EmailAddress().WithMessage("Formato de email incorrecto");

            RuleFor(p => p.Telefono)
                .MaximumLength(15).WithMessage("El teléfono no puede superar los 15 caracteres");

            RuleFor(p => p.TipoDocumento).IsInEnum();
            RuleFor(p => p.Genero).IsInEnum();
            RuleFor(p => p.GrupoSanguineo).IsInEnum();

        }
    }
}