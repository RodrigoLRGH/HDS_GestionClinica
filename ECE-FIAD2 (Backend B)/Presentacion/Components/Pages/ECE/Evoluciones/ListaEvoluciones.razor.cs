using Aplicacion.DTOs.Evoluciones;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Presentacion.Servicios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentacion.Components.Pages.ECE.Evoluciones
{
    public partial class ListaEvoluciones : ComponentBase
    {
        [Inject] private IEvolucionService EvolucionService { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        private bool cargando = true;
        private List<EvolucionDTO> evoluciones = new();

        protected override async Task OnInitializedAsync() => await CargarEvoluciones();

        private void NavegarACrear() => Navigation.NavigateTo("/crear-evolucion");


        private async Task CargarEvoluciones()
        {
            cargando = true;
            var resultado = await EvolucionService.ObtenerTodosAsync();
            if (resultado.Exitoso && resultado.Datos != null)
                evoluciones = new List<EvolucionDTO>(resultado.Datos);
            else
                await Toastr.MsgError("Error al cargar evoluciones: " + resultado.Mensaje);
            cargando = false;
        }

        private void VerDetalles(int id) => Navigation.NavigateTo($"/detalles-evolucion/{id}");
        private void Editar(int id) => Navigation.NavigateTo($"/editar-evolucion/{id}");

        private async Task ConfirmarEliminar(int id, string pacienteNombre)
        {
            var confirmado = await JSRuntime.InvokeAsync<bool>("SweetAlert.showConfirm",
                "¿Eliminar evolución?",
                $"La evolución de {pacienteNombre} se eliminará lógicamente.",
                "warning", "Sí, eliminar");
            if (confirmado)
            {
                var resultado = await EvolucionService.EliminarAsync(id);
                if (resultado.Exitoso)
                {
                    await Toastr.MsgExito("Evolución eliminada.");
                    await CargarEvoluciones();
                    StateHasChanged();
                }
                else await Toastr.MsgError("Error: " + resultado.Mensaje);
            }
        }
    }
}