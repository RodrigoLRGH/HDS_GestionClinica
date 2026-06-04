using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Aplicacion.DTOs.Doctores;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Doctores
{
    public partial class ListaDoctores : ComponentBase
    {
        [Inject] private IDoctorService ServiciosDoctor { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        protected List<DoctorDTO>? doctores;

        protected override async Task OnInitializedAsync()
        {
            await CargarDoctores();
        }

        protected async Task CargarDoctores()
        {
            var resultado = await ServiciosDoctor.ObtenerTodosAsync();
            if (resultado.Exitoso && resultado.Datos != null)
                doctores = resultado.Datos.ToList();
            else
            {
                await Toastr.MsgError("Error al cargar los doctores: " + resultado.Mensaje);
                doctores = new List<DoctorDTO>();
            }
        }

        protected void NavegarACrear() => Navigation.NavigateTo("/crear-doctor");
        protected void VerDetalles(int id) => Navigation.NavigateTo($"/detalles-doctor/{id}");
        protected void Editar(int id) => Navigation.NavigateTo($"/editar-doctor/{id}");

        protected async Task ConfirmarEliminar(int id, string nombres, string apellidos, int cantidadCitas)
        {
            string nombreCompleto = $"{nombres} {apellidos}";

            if (cantidadCitas > 0)
            {
                await JSRuntime.InvokeVoidAsync("Swal.fire",
                    "No se puede eliminar",
                    $"El doctor {nombreCompleto} tiene {cantidadCitas} cita(s) asociada(s). " +
                    "Primero debe reasignar o eliminar las citas antes de continuar.",
                    "warning");
                return;
            }

            var confirmado = await JSRuntime.InvokeAsync<bool>("SweetAlert.showConfirm",
                "¿Estás seguro de borrar?",
                $"Los datos del doctor {nombreCompleto} se eliminarán de forma permanente del sistema.",
                "warning",
                "Sí, eliminar");

            if (confirmado)
            {
                var resultado = await ServiciosDoctor.EliminarAsync(id);
                if (resultado.Exitoso)
                {
                    await Toastr.MsgExito($"Doctor {nombreCompleto} eliminado correctamente.");
                    await CargarDoctores();
                    StateHasChanged();
                }
                else
                    await Toastr.MsgError("Error al eliminar: " + resultado.Mensaje);
            }
            else
            {
                await Toastr.MsgInformacion($"Acción cancelada: Los datos del doctor {nombreCompleto} fueron conservados.");
            }
        }
    }
}