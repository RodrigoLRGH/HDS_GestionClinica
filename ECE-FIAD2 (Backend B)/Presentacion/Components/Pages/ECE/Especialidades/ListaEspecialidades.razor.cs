using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Aplicacion.DTOs.Especialidades;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Especialidades
{
    public partial class ListaEspecialidades : ComponentBase
    {
        [Inject] private IEspecialidadService serviciosEspecialidad { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        protected List<EspecialidadDTO>? especialidades;

        protected override async Task OnInitializedAsync() => await CargarEspecialidades();

        protected async Task CargarEspecialidades()
        {
            var resultado = await serviciosEspecialidad.ObtenerTodosAsync();
            if (resultado.Exitoso && resultado.Datos != null)
                especialidades = resultado.Datos.ToList();
            else
            {
                await Toastr.MsgError("Error al cargar las especialidades: " + resultado.Mensaje);
                especialidades = new();
            }
        }

        protected void NavegarACrear() => Navigation.NavigateTo("/crear-especialidad");
        protected void VerDetalles(int id) => Navigation.NavigateTo($"/detalles-especialidad/{id}");
        protected void Editar(int id) => Navigation.NavigateTo($"/editar-especialidad/{id}");

        protected async Task ConfirmarEliminar(int id, string nombre, int cantidadMedicos)
        {
            if (cantidadMedicos > 0)
            {
                await Toastr.MsgError($"No se puede eliminar '{nombre}' porque tiene {cantidadMedicos} médico(s) asociado(s). Reasigne o elimine los médicos primero.");
                return;
            }

            var confirmado = await JSRuntime.InvokeAsync<bool>("SweetAlert.showConfirm",
                "¿Estás seguro de borrar?",
                $"La especialidad '{nombre}' se eliminará permanentemente.",
                "warning", "Sí, eliminar");

            if (confirmado)
            {
                var resultado = await serviciosEspecialidad.EliminarAsync(id);
                if (resultado.Exitoso)
                {
                    await Toastr.MsgExito($"Especialidad {nombre} eliminada correctamente.");
                    await CargarEspecialidades();
                }
                else
                    await Toastr.MsgError("Error al eliminar: " + resultado.Mensaje);
            }
            else
                await Toastr.MsgInformacion($"Acción cancelada para {nombre}.");
        }
    }
}