using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Aplicacion.DTOs.Pacientes;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;
using Aplicacion.DTOs.Especialidades;
using Aplicacion.Helpers;
namespace Presentacion.Components.Pages.ECE.Especialidades
{
    public partial class ListaEspecialidades : ComponentBase
    {
        [Inject] private IEspecialidadService serviciosEspecialidad { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        protected List<EspecialidadDTO>? especialidades;

        protected override async Task OnInitializedAsync()
        {
            await CargarEspecialidades();
        }

        protected async Task CargarEspecialidades()
        {
            var resultado = await serviciosEspecialidad.ObtenerTodosAsync();
            if (resultado.Exitoso && resultado.Datos != null)
            {
                especialidades = resultado.Datos.ToList();
            }
            else
            {
                await Toastr.MsgError("Error al cargar las especialidades");
                especialidades = new List<EspecialidadDTO>();
            }
        }

        protected void NavegarACrear() => Navigation.NavigateTo("crear-especialidad");
        protected void VerDetalles(int id) => Navigation.NavigateTo($"/detalles-especialidad/{id}");
        protected void Editar(int id) => Navigation.NavigateTo($"/editar-especialidad/{id}");

        protected async Task ConfirmarEliminar(int id, string nombre)
        {
            string nombreCompleto = $"{nombre}";
            string mensajeConfirmacion = $"Los datos de la especialidad: {nombre} se eliminara permanentemente del sistema";
            var confirmado = await JSRuntime.InvokeAsync<bool>("SweetAlert.showConfirm",
                "¿Estas seguro de borrar?",
                mensajeConfirmacion,
                "warning",
                "Si, eliminar");
            if (confirmado)
            {
                var resultado = await serviciosEspecialidad.EliminarAsync(id);
                if(resultado.Exitoso)
                {
                    await Toastr.MsgExito($"Especialidad {nombre} eliminado correctamente");
                    await CargarEspecialidades();
                    StateHasChanged();
                }
                else
                {
                    await Toastr.MsgInformacion($"Accion cancelada: Los datos de la especialidad {nombre} fueron conservados, hay doctor/es asociado/s");
                }
            }
            else
            {
                await Toastr.MsgInformacion($"Accion cancelada: Los datos de la especialidad {nombre} fueron conservados");
            }
        }
    }
}
