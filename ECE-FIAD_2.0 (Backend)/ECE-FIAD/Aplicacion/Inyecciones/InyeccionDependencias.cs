using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aplicacion.Servicios.Interfaces;
using Aplicacion.Servicios.Implementaciones;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace Aplicacion.Inyecciones
{
    public static class InyeccionDependencias
    {
        public static IServiceCollection AgregarAplicacion(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(typeof(InyeccionDependencias).Assembly);
            // FluentValidation: registra todos los validadores del ensamblado actual
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            // Servicios
            services.AddScoped<IPacienteService, PacienteService>();
            services.AddScoped<IEspecialidadService, EspecialidadService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<ICitaService, CitaService>();
            services.AddScoped<IHistoriaClinicaService, HistoriaClinicaService>();
            services.AddScoped<IEvolucionService, EvolucionService>();
            return services;
        }
    }
}