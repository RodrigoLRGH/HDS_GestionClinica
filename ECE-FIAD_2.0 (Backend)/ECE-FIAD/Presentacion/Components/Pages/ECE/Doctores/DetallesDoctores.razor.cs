using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.Especialidades;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Doctores
{
    public partial class DetallesDoctores
    {
        // Inyección de dependencias, asegurando que las propiedades sean no nulas,
        // para evitar errores en tiempo de ejecución.
        [Inject] private IDoctorService serviciosDoctor { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;
        [Inject] private IEspecialidadService serviciosEspecialidades { get; set; } = null!;
        [Parameter] public int id { get; set; }
        private DoctorDTO? doctor;
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
            await CargarPaciente();
        }
        private async Task CargarPaciente()
        {
            var resultado = await serviciosDoctor.ObtenerPorIdAsync(id);
            if (resultado.Exitoso && resultado.Datos != null)
            {
                doctor = resultado.Datos;
            }
            else
            {
                await Toastr.MsgError("Doctor no encontrado o error al cargar.");
                Navigation.NavigateTo("/doctores");
            }
        }
        private void Volver() => Navigation.NavigateTo("/doctores");
        private void Editar() => Navigation.NavigateTo($"/editar-doctor/{id}");
    }
}
