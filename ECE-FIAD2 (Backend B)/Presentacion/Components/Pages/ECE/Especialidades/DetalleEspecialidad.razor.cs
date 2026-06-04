using Microsoft.AspNetCore.Components;
using Aplicacion.DTOs.Especialidades;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Especialidades
{
    public partial class DetalleEspecialidad : ComponentBase
    {
        [Parameter] public int Id { get; set; }

        [Inject] private IEspecialidadService serviciosEspecialidad { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        protected EspecialidadDTO? especialidad;

        protected override async Task OnInitializedAsync()
        {
            var resultado = await serviciosEspecialidad.ObtenerPorIdAsync(Id);
            if (resultado.Exitoso && resultado.Datos != null)
                especialidad = resultado.Datos;
            else
            {
                await Toastr.MsgError("La especialidad no existe.");
                Navigation.NavigateTo("/especialidades");
            }
        }

        protected void VolverAlListado() => Navigation.NavigateTo("/especialidades");
        protected void EditarEspecialidad() => Navigation.NavigateTo($"/editar-especialidad/{Id}");
    }
}