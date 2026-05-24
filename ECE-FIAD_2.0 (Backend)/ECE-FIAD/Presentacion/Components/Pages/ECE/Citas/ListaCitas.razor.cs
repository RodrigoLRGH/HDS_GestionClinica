using Aplicacion.DTOs.Citas;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Citas
{
    partial class ListaCitas
    {
        [Inject] private ICitaService serviciosCita { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        protected bool EstiloCard = true;
        protected List<CitaDTO>? citas;

        protected override async Task OnInitializedAsync()
        {
            await CargarCitas();
        }

        protected async Task CargarCitas()
        {
            var resultado = await serviciosCita.ObtenerTodosAsync();

            if (resultado.Exitoso && resultado.Datos != null)
            {
                citas = resultado.Datos.ToList();
            }
            else
            {
                await Toastr.MsgError("Error al cargar las citas: " + resultado.Mensaje);
                citas = new List<CitaDTO>();
            }
        }

        protected void CambiaEstilo()
        {
            EstiloCard = !EstiloCard;
        }

        protected void NavegarACrear() => Navigation.NavigateTo("/crear-cita");
        protected void VerDetalles(int id) => Navigation.NavigateTo($"/detalles-cita/{id}");
        protected void Editar(int id) => Navigation.NavigateTo($"/editar-cita/{id}");

        protected async Task ConfirmarEliminar(CitaDTO cita)
        {
            string descripcion = $"Paciente: {cita.Paciente.Nombres} - Doctor: {cita.Doctor.Nombres} - Fecha: {cita.FechaHora:dd/MM/yyyy HH:mm}";

            string mensajeConfirmacion = $"La cita ({descripcion}) será eliminada del sistema.";

            var confirmado = await JSRuntime.InvokeAsync<bool>("SweetAlert.showConfirm",
                "¿Estás seguro de borrar?",
                mensajeConfirmacion,
                "warning",
                "Sí, eliminar");

            if (confirmado)
            {
                var resultado = await serviciosCita.EliminarAsync(cita.Id);

                if (resultado.Exitoso)
                {
                    await Toastr.MsgExito("Cita eliminada correctamente.");
                    await CargarCitas();
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
    }
}