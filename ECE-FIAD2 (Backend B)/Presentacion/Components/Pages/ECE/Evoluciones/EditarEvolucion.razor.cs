using Microsoft.AspNetCore.Components;
using Aplicacion.DTOs.Evoluciones;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Evoluciones
{
    public partial class EditarEvolucion : ComponentBase
    {
        [Inject] private IEvolucionService EvolucionService { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        [Parameter] public int id { get; set; }

        private EvolucionDTO? evolucionOriginal;
        private ActualizarEvolucionDTO evolucionEditar = new();
        private bool cargando = true;
        private bool procesando = false;

        protected override async Task OnInitializedAsync()
        {
            await CargarEvolucion();
        }

        private async Task CargarEvolucion()
        {
            cargando = true;
            var resultado = await EvolucionService.ObtenerPorIdAsync(id);
            if (resultado.Exitoso && resultado.Datos != null)
            {
                evolucionOriginal = resultado.Datos;
                evolucionEditar = new ActualizarEvolucionDTO
                {
                    Id = evolucionOriginal.Id,
                    Diagnostico = evolucionOriginal.Diagnostico,
                    Tratamiento = evolucionOriginal.Tratamiento,
                    Notas = evolucionOriginal.Notas,
                    Activo = evolucionOriginal.Activo
                };
            }
            else
            {
                await Toastr.MsgError("Evolución no encontrada.");
                Navigation.NavigateTo("/evoluciones");
            }
            cargando = false;
        }

        private async Task GrabarEvolucion()
        {
            procesando = true;
            try
            {
                var resultado = await EvolucionService.ActualizarAsync(evolucionEditar);
                if (resultado.Exitoso)
                {
                    await Toastr.MsgExito("Evolución actualizada correctamente.");
                    Navigation.NavigateTo("/evoluciones");
                }
                else
                {
                    await Toastr.MsgError("Error: " + resultado.Mensaje);
                }
            }
            finally
            {
                procesando = false;
            }
        }

        private void Cancelar() => Navigation.NavigateTo("/evoluciones");
    }
}