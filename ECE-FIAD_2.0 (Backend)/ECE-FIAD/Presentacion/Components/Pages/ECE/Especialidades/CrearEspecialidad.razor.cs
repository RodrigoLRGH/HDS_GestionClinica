using Aplicacion.DTOs.Especialidades;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Components;
using Presentacion.Servicios;


namespace Presentacion.Components.Pages.ECE.Especialidades
{
    public partial class CrearEspecialidad
    {
        [Inject] private IEspecialidadService serviciosEspecialidades { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;
        protected CrearEspecialidadDTO especialidadDto = new();
        protected async Task GrabarEspecialidad()
        {
            var resultado = await serviciosEspecialidades.CrearAsync(especialidadDto);
            if (resultado.Exitoso)
            {
                await Toastr.MsgExito("Especialidad creada exitosamente.");
                Navigation.NavigateTo("/especialidades");
            }
            else
            {
                await Toastr.MsgError("Error al crear: " + resultado.Mensaje);
            }
        }
        protected void Cancelar() => Navigation.NavigateTo("/especialidades");
    }
}