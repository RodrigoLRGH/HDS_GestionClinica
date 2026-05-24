using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.HistoriasClinicas
{
    partial class ListaHistoriasClinicas
    {
        [Inject] private IHistoriaClinicaService serviciosHistoriaClinica { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        protected List<HistoriaClinicaDTO>? historiasClinicas;

        protected override async Task OnInitializedAsync()
        {
            await CargarHistoriasClinicas();
        }

        protected async Task CargarHistoriasClinicas()
        {
            historiasClinicas = new List<HistoriaClinicaDTO>();
            StateHasChanged();

            var resultado = await serviciosHistoriaClinica.ObtenerTodosAsync();
            if (resultado.Exitoso && resultado.Datos != null)
            {
                historiasClinicas = resultado.Datos.ToList();
                StateHasChanged();
            }
            else
            {
                await Toastr.MsgError("Error al cargar las historias clinicas: " + resultado.Mensaje);
                historiasClinicas = new List<HistoriaClinicaDTO>();
            }
        }

        protected void NavegarACrear() => Navigation.NavigateTo("/crear-historia");
        protected void VerDetalles(int id) => Navigation.NavigateTo($"/detalles-historia/{id}");
        protected void Editar(int id) => Navigation.NavigateTo($"/editar-historia/{id}");

        protected async Task ConfirmarEliminar(HistoriaClinicaDTO historiaClinica)
        {
            string descripcion = $"Paciente: {historiaClinica.Paciente.Nombres} - Alergias: {historiaClinica.Alergias}";

            string mensajeConfirmacion = $"La cita ({descripcion}) será eliminada del sistema.";

            var confirmado = await JSRuntime.InvokeAsync<bool>("SweetAlert.showConfirm",
                "¿Estás seguro de borrar?",
                mensajeConfirmacion,
                "warning",
                "Sí, eliminar");

            if (confirmado)
            {
                var resultado = await serviciosHistoriaClinica.EliminarAsync(historiaClinica.Id);

                if (resultado.Exitoso)
                {
                    await Toastr.MsgExito("Historia clinica eliminada correctamente.");
                    await CargarHistoriasClinicas();
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