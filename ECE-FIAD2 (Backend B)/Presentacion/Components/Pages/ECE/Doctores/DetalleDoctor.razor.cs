using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Aplicacion.DTOs.Doctores;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Doctores
{
    public partial class DetalleDoctor : ComponentBase
    {
        [Inject] private IDoctorService ServiciosDoctor { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        [Parameter] public int id { get; set; }

        private DoctorDTO? doctor;

        protected override async Task OnInitializedAsync()
        {
            await CargarDoctor();
        }

        private async Task CargarDoctor()
        {
            var resultado = await ServiciosDoctor.ObtenerPorIdAsync(id);
            if (resultado.Exitoso && resultado.Datos != null)
                doctor = resultado.Datos;
            else
            {
                await Toastr.MsgError("Doctor no encontrado o error al cargar.");
                Navigation.NavigateTo("/doctores");
            }
        }

        private void Volver() => Navigation.NavigateTo("/doctores");
        private void Editar() => Navigation.NavigateTo($"/editar-doctor/{id}");
    }
}