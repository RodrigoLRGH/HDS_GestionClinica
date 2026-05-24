using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.Especialidades;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Doctores
{
    public partial class ListaDoctores
    {
        [Inject] private IDoctorService serviciosDoctor { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;
        [Inject] private IEspecialidadService serviciosEspecialidades { get; set; } = null!;
        // Propiedad para controlar el estilo de visualización (card o tabla)
        protected bool EstiloCard = true;
        protected List<DoctorDTO>? doctores;
        protected List<EspecialidadDTO>? especialidades = new();
        protected override async Task OnInitializedAsync()
        {
            var resultado = await serviciosEspecialidades.ObtenerTodosAsync();
            if (resultado.Exitoso && resultado.Datos != null)
            {
                especialidades = resultado.Datos.ToList();
            }
            else
            {
                await Toastr.MsgError("Error al cargar especialidades");
                especialidades = new List<EspecialidadDTO>();
            }
            await CargarDoctores();
        }
        protected async Task CargarDoctores()
        {
            var resultado = await serviciosDoctor.ObtenerTodosAsync();
            if (resultado.Exitoso && resultado.Datos != null)
            {
                doctores = resultado.Datos.ToList();
            }
            else
            {
                await Toastr.MsgError("Error al cargar los doctores: " + resultado.Mensaje);
                doctores = new List<DoctorDTO>();
            }
        }
        protected void CambiaEstilo()
        {
            EstiloCard = !EstiloCard;
        }
        // Métodos de navegación para crear, ver detalles y editar pacientes
        protected void NavegarACrear() => Navigation.NavigateTo("/crear-doctor");
        protected void VerDetalles(int id) => Navigation.NavigateTo($"/detalles-doctor/{id}");
        protected void Editar(int id) => Navigation.NavigateTo($"/editar-doctor/{id}");
        protected async Task ConfirmarEliminar(int id, string nombres, string apellidos)
        {
            // Construir el mensaje incluyendo el nombre completo del paciente
            string nombreCompleto = $"{nombres} {apellidos}";
            string mensajeConfirmacion = $"Los datos del doctor: {nombreCompleto} se eliminarán de forma permanente del sistemas.";
            var confirmado = await JSRuntime.InvokeAsync<bool>("SweetAlert.showConfirm",
            "¿Estás seguro de borrar?",
            mensajeConfirmacion,
            "warning",
            "Sí, eliminar");
            if (confirmado)
            {
                var resultado = await serviciosDoctor.EliminarAsync(id);
                if (resultado.Exitoso)
                {
                    await Toastr.MsgExito($"Doctor {nombreCompleto} eliminado correctamente.");
                    await CargarDoctores();
                    StateHasChanged();
                }
                else
                {
                    await Toastr.MsgError("Error al eliminar: " + resultado.Mensaje);
                }
            }
            else
            {
                await Toastr.MsgInformacion($"Acción cancelada: Los datos del doctor {nombreCompleto} fueron conservados.");
            }
        }
    }
}
