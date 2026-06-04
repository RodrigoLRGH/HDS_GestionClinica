using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentacion.Components.Pages.ECE.HistoriasClinicas
{
    public partial class ListaHistorias : ComponentBase
    {
        [Inject] private IHistoriaClinicaService HistoriaService { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        private bool cargando = true;
        private List<HistoriaClinicaDTO> historias = new();

        protected override async Task OnInitializedAsync()
        {
            await CargarHistorias();
        }

        private async Task CargarHistorias()
        {
            cargando = true;
            var resultado = await HistoriaService.ObtenerTodosAsync();
            if (resultado.Exitoso && resultado.Datos != null)
                historias = resultado.Datos.ToList();
            else
                await Toastr.MsgError("Error al cargar historias: " + resultado.Mensaje);
            cargando = false;
        }

        private void NavegarACrear() => Navigation.NavigateTo("/crear-historia");
        private void VerDetalles(int id) => Navigation.NavigateTo($"/detalles-historia/{id}");
        private void Editar(int id) => Navigation.NavigateTo($"/editar-historia/{id}");

        private async Task ConfirmarEliminar(int id, string pacienteNombre)
        {
            var confirmado = await JSRuntime.InvokeAsync<bool>("SweetAlert.showConfirm",
                "¿Eliminar historia clínica?",
                $"La historia clínica de {pacienteNombre} se eliminará lógicamente.",
                "warning", "Sí, eliminar");
            if (confirmado)
            {
                var resultado = await HistoriaService.EliminarAsync(id);
                if (resultado.Exitoso)
                {
                    await Toastr.MsgExito("Historia eliminada.");
                    await CargarHistorias();
                    StateHasChanged();
                }
                else
                    await Toastr.MsgError("Error: " + resultado.Mensaje);
            }
        }
    }
}