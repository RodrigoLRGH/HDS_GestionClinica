using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Aplicacion.DTOs.Citas;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentacion.Components.Pages.ECE.Citas
{
    public partial class ListaCitas : ComponentBase
    {
        [Inject] private ICitaService CitaService { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        private bool cargando = true;
        private List<CitaDTO> citas = new();

        protected override async Task OnInitializedAsync()
        {
            await CargarCitas();
        }

        private async Task CargarCitas()
        {
            cargando = true;
            var resultado = await CitaService.ObtenerTodosAsync();
            if (resultado.Exitoso && resultado.Datos != null)
                citas = resultado.Datos.ToList();
            else
                await Toastr.MsgError("Error al cargar citas: " + resultado.Mensaje);
            cargando = false;
        }

        private void NavegarACrear() => Navigation.NavigateTo("/crear-cita");
        private void VerDetalles(int id) => Navigation.NavigateTo($"/detalles-cita/{id}");
        private void Editar(int id) => Navigation.NavigateTo($"/editar-cita/{id}");

        private async Task ConfirmarEliminar(int id, string pacienteNombre)
        {
            var confirmado = await JSRuntime.InvokeAsync<bool>("SweetAlert.showConfirm",
                "¿Eliminar cita?",
                $"La cita de {pacienteNombre} se eliminará lógicamente.",
                "warning", "Sí, eliminar");
            if (confirmado)
            {
                var resultado = await CitaService.EliminarAsync(id);
                if (resultado.Exitoso)
                {
                    await Toastr.MsgExito("Cita eliminada.");
                    await CargarCitas();
                    StateHasChanged();
                }
                else
                    await Toastr.MsgError("Error: " + resultado.Mensaje);
            }
        }
    }
}