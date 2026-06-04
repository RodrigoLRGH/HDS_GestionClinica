using Microsoft.AspNetCore.Components;
using Aplicacion.DTOs.Citas;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Citas  
{
    public partial class DetalleCita : ComponentBase
    {
        [Inject] private ICitaService CitaService { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        [Parameter] public int id { get; set; }

        private CitaDTO? cita;         
        private bool cargando = true;   

        protected override async Task OnInitializedAsync()
        {
            await CargarCita();
        }

        private async Task CargarCita()
        {
            cargando = true;
            StateHasChanged();

            var resultado = await CitaService.ObtenerPorIdAsync(id);

            if (resultado.Exitoso && resultado.Datos != null)
            {
                cita = resultado.Datos;
            }
            else
            {
                await Toastr.MsgError("Cita no encontrada o error al cargar.");
                Navigation.NavigateTo("/citas");
            }

            cargando = false;
            StateHasChanged();
        }

        private void Volver() => Navigation.NavigateTo("/citas");
        private void Editar() => Navigation.NavigateTo($"/editar-cita/{id}");
    }
}