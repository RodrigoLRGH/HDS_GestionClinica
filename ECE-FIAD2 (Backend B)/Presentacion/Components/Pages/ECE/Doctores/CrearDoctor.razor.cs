using Microsoft.AspNetCore.Components;
using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.Especialidades;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Doctores
{
    public partial class CrearDoctor : ComponentBase
    {
        [Inject] private IDoctorService ServiciosDoctor { get; set; } = null!;
        [Inject] private IEspecialidadService ServiciosEspecialidad { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        protected CrearDoctorDTO doctorDto = new();
        protected List<EspecialidadDTO> especialidades = new();

        protected override async Task OnInitializedAsync()
        {
            var resultado = await ServiciosEspecialidad.ObtenerTodosAsync();
            if (resultado.Exitoso && resultado.Datos != null)
                especialidades = resultado.Datos.Where(e => e.Activo).ToList();
        }

        protected async Task GrabarDoctor()
        {
            var resultado = await ServiciosDoctor.CrearAsync(doctorDto);
            if (resultado.Exitoso)
            {
                await Toastr.MsgExito("Doctor creado exitosamente.");
                Navigation.NavigateTo("/doctores");
            }
            else
                await Toastr.MsgError("Error al crear: " + resultado.Mensaje);
        }

        protected void Cancelar() => Navigation.NavigateTo("/doctores");
    }
}