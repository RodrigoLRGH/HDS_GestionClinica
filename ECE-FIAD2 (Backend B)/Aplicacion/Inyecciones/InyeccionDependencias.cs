using Aplicacion.Servicios.Implementaciones;
using Aplicacion.Servicios.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Aplicacion.Inyecciones
{
    public static class InyeccionDependencias
    {
        public static IServiceCollection AgregarAplicacion(this IServiceCollection servicios)
        {
            servicios.AddAutoMapper(Assembly.GetExecutingAssembly());
            servicios.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            servicios.AddScoped<IPacienteService, PacienteService>();
            servicios.AddScoped<IEspecialidadService, EspecialidadService>();
            servicios.AddScoped<IDoctorService, DoctorService>();
            servicios.AddScoped<ICitaService, CitaService>();
            servicios.AddScoped<IHistoriaClinicaService, HistoriaClinicaService>();
            servicios.AddScoped<IEvolucionService, EvolucionService>();

            return servicios;
        }
    }
}