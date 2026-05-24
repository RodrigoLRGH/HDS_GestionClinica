using Aplicacion.DTOs.Evoluciones;
using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Evoluciones
{
    partial class ListaEvoluciones
    {
        [Inject] private IEvolucionService serviciosEvolucion { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;
        [Inject] private IHistoriaClinicaService serviciosHistoriaClinica { get; set; } = null!;

        protected List<EvolucionDTO>? evoluciones = new();
        protected List<EvolucionDTO>? evolucionesFiltradas = new();
        protected List<HistoriaClinicaDTO> historias = new();

        protected override async Task OnInitializedAsync()
        {
            await CargarEvoluciones();

            var h = await serviciosHistoriaClinica.ObtenerTodosAsync();
            if (h.Exitoso && h.Datos != null)
                historias = h.Datos.ToList();
        }

        protected async Task CargarEvoluciones()
        {
            var resultado = await serviciosEvolucion.ObtenerTodosAsync();

            if (resultado.Exitoso && resultado.Datos != null)
            {
                evoluciones = resultado.Datos.ToList();
                evolucionesFiltradas = evoluciones;
            }
            else
            {
                await Toastr.MsgError("Error al cargar las evoluciones: " + resultado.Mensaje);
                evoluciones = new List<EvolucionDTO>();
            }
        }

        protected void FiltrarPorHistoria(ChangeEventArgs e)
        {
            var idHistoria = int.Parse(e.Value?.ToString() ?? "0");

            if (idHistoria == 0)
                evolucionesFiltradas = evoluciones;
            else
                evolucionesFiltradas = evoluciones
                    .Where(ev => ev.IdHistoriaClinica == idHistoria)
                    .ToList();

            StateHasChanged();
        }
        protected void NavegarACrear() => Navigation.NavigateTo("/crear-evolucion");
        protected void VerDetalles(int id) => Navigation.NavigateTo($"/detalles-evolucion/{id}");
        protected void Editar(int id) => Navigation.NavigateTo($"/editar-evolucion/{id}");

        protected async Task ConfirmarEliminar(EvolucionDTO evolucion)
        {
            string descripcion = $"Doctor: {evolucion.Doctor.Nombres} - Diagnostico: {evolucion.Diagnostico}";

            string mensajeConfirmacion = $"La evolucion ({descripcion}) será eliminada del sistema.";

            var confirmado = await JSRuntime.InvokeAsync<bool>("SweetAlert.showConfirm",
                "¿Estás seguro de borrar?",
                mensajeConfirmacion,
                "warning",
                "Sí, eliminar");

            if (confirmado)
            {
                var resultado = await serviciosEvolucion.EliminarAsync(evolucion.Id);

                if (resultado.Exitoso)
                {
                    await Toastr.MsgExito("Evolucion eliminada correctamente.");
                    await CargarEvoluciones();
                    StateHasChanged();
                }
                else
                {
                    await Toastr.MsgError("Error al eliminar: " + resultado.Mensaje);
                }
            }
            else
            {
                await Toastr.MsgInformacion("Acción cancelada.");
            }
        }

        protected string Truncar(string? texto, int longitud = 30)
        {
            if (string.IsNullOrEmpty(texto)) return "—";
            return texto.Length > longitud ? texto.Substring(0, longitud) + "..." : texto;
        }

    }
}