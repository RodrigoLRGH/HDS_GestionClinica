using Microsoft.AspNetCore.Components;
using Aplicacion.DTOs.Evoluciones;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Evoluciones
{
    public partial class DetalleEvolucion : ComponentBase
    {
        [Inject] private IEvolucionService EvolucionService { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        [Parameter] public int id { get; set; }

        private EvolucionDTO? evolucion;
        private bool cargando = true;

        protected override async Task OnInitializedAsync()
        {
            await CargarEvolucion();
        }

        private async Task CargarEvolucion()
        {
            cargando = true;
            var resultado = await EvolucionService.ObtenerPorIdAsync(id);
            if (resultado.Exitoso && resultado.Datos != null)
                evolucion = resultado.Datos;
            else
                await Toastr.MsgError("Evolución no encontrada.");
            cargando = false;
        }

        private void Volver() => Navigation.NavigateTo("/evoluciones");
        private void Editar() => Navigation.NavigateTo($"/editar-evolucion/{id}");
    }
}