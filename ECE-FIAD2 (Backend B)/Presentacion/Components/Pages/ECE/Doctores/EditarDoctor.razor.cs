using Microsoft.AspNetCore.Components;
using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.Especialidades;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Doctores
{
    public partial class EditarDoctor : ComponentBase
    {
        [Parameter] public int id { get; set; }

        private DoctorDTO? doctorOriginal;
        private ActualizarDoctorDTO doctorEditar = new();
        private List<EspecialidadDTO> especialidades = new();

        protected override async Task OnInitializedAsync()
        {
            var resEsp = await EspecialidadService.ObtenerTodosAsync();
            if (resEsp.Exitoso && resEsp.Datos != null)
                especialidades = resEsp.Datos.Where(e => e.Activo).ToList();

            await CargarDoctor();
        }

        private async Task CargarDoctor()
        {
            var resultado = await DoctorService.ObtenerPorIdAsync(id);
            if (resultado.Exitoso && resultado.Datos != null)
            {
                doctorOriginal = resultado.Datos;

                doctorEditar.Id = doctorOriginal.Id;
                doctorEditar.Nombres = doctorOriginal.Nombres;
                doctorEditar.Apellidos = doctorOriginal.Apellidos;
                doctorEditar.IdEspecialidad = doctorOriginal.IdEspecialidad;
                doctorEditar.Telefono = doctorOriginal.Telefono ?? string.Empty;
                doctorEditar.Email = doctorOriginal.Email ?? string.Empty;
                doctorEditar.HorarioAtencion = doctorOriginal.HorarioAtencion ?? string.Empty;
                doctorEditar.FechaContratacion = doctorOriginal.FechaContratacion;
            }
            else
            {
                await Toastr.MsgInformacion("Doctor no encontrado o error al cargar.");
                Navigation.NavigateTo("/doctores");
            }
        }

        private async Task GrabarDoctor()
        {
            var resultado = await DoctorService.ActualizarAsync(doctorEditar);
            if (resultado.Exitoso)
            {
                await Toastr.MsgExito("Doctor actualizado exitosamente.");
                Navigation.NavigateTo("/doctores");
            }
            else
                await Toastr.MsgError("Error al actualizar: " + resultado.Mensaje);
        }

        private void Cancelar() => Navigation.NavigateTo("/doctores");
    }
}