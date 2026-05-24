using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.Especialidades;
using Aplicacion.DTOs.Pacientes;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Components;
using Presentacion.Components.Pages.ECE.Especialidades;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Doctores
{
    public partial class CrearDoctor
    {
        [Inject] private IDoctorService serviciosDoctor { get; set; } = null!;
        [Inject] private IEspecialidadService serviciosEspecialidades { get; set; } = null!;
        [Inject] private IToastrService Toastr {  get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;
        protected CrearDoctorDTO doctorDto = new();
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
        }

        protected async Task GrabarDoctor()
        {
            //doctorDto.Especialidad = especialidades.FirstOrDefault(e => e.Id == doctorDto.IdEspecialidad);
            var resultado = await serviciosDoctor.CrearAsync(doctorDto);
            if (resultado.Exitoso)
            {
                await Toastr.MsgExito("Doctor creado exitosamente");
                Navigation.NavigateTo("/doctores");
            }    
            else
            {
                await Toastr.MsgError("Error al crear: " + resultado.Mensaje);
            }
        }

        protected void Cancelar() => Navigation.NavigateTo("/doctores");
    }
}
