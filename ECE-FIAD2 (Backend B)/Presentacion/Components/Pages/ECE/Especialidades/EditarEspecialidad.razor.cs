//using Microsoft.AspNetCore.Components;
//using Aplicacion.DTOs.Especialidades;
using Microsoft.AspNetCore.Components;
using Aplicacion.DTOs.Especialidades;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Especialidades
{
    public partial class EditarEspecialidad : ComponentBase
    {
        [Parameter] public int id { get; set; }  

        [Inject] private IEspecialidadService EspecialidadService { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        private EspecialidadDTO? especialidadOriginal;
        private ActualizarEspecialidadDTO especialidadEditar = new();
        private bool cargando = true;

        protected override async Task OnInitializedAsync()
        {
            var resultado = await EspecialidadService.ObtenerPorIdAsync(id);
            if (resultado.Exitoso && resultado.Datos != null)
            {
                especialidadOriginal = resultado.Datos;
                especialidadEditar.Nombre = especialidadOriginal.Nombre;
                especialidadEditar.Descripcion = especialidadOriginal.Descripcion;
            }
            else
            {
                await Toastr.MsgError("Especialidad no encontrada.");
                Navigation.NavigateTo("/especialidades");
                return;
            }
            cargando = false;
        }

        private async Task GrabarEspecialidad()
        {
            especialidadEditar.Id = id;
            var resultado = await EspecialidadService.ActualizarAsync(especialidadEditar);
            if (resultado.Exitoso)
            {
                await Toastr.MsgExito("Especialidad actualizada.");
                Navigation.NavigateTo("/especialidades");
            }
            else
            {
                await Toastr.MsgError("Error: " + resultado.Mensaje);
            }
        }

        private void Cancelar() => Navigation.NavigateTo("/especialidades");
    }
}