using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Aplicacion.DTOs.Pacientes;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Pacientes
{
    public partial class ListaPacientes : ComponentBase
    {
        [Inject] private IPacienteService ServiciosPaciente { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        protected List<PacienteDTO>? pacientes;
        protected bool EstiloCard = true;

        protected override async Task OnInitializedAsync() => await CargarPacientes();

        private async Task CargarPacientes()
        {
            var resultado = await ServiciosPaciente.ObtenerTodosAsync();
            if (resultado.Exitoso && resultado.Datos != null)
                pacientes = resultado.Datos.ToList();
            else
                await Toastr.MsgError("Error al cargar: " + resultado.Mensaje);
        }

        protected void NavegarACrear() => Navigation.NavigateTo("/crear-paciente");
        protected void VerDetalles(int id) => Navigation.NavigateTo($"/detalles-paciente/{id}");
        protected void Editar(int id) => Navigation.NavigateTo($"/editar-paciente/{id}");
        protected void CambiaEstilo() => EstiloCard = !EstiloCard;

        protected async Task ConfirmarEliminar(int id, string nombres, string apellidos)
        {
            var confirmado = await JSRuntime.InvokeAsync<bool>("SweetAlert.showConfirm",
                "¿Eliminar paciente?", $"¿Borrar a {nombres} {apellidos}?", "warning", "Sí, eliminar");
            if (confirmado)
            {
                var resultado = await ServiciosPaciente.EliminarAsync(id);
                if (resultado.Exitoso)
                {
                    await Toastr.MsgExito("Paciente eliminado.");
                    await CargarPacientes();
                    StateHasChanged();
                }
                else await Toastr.MsgError("Error: " + resultado.Mensaje);
            }
        }
    }
}