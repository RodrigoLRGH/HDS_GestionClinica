using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.Especialidades;
using Aplicacion.Servicios.Implementaciones;
using Aplicacion.Servicios.Interfaces;
using Dominio.Entidades.Especialidades;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Presentacion.Components.Pages.ECE.Especialidades;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace Presentacion.Components.Pages.ECE.Doctores
{
    public partial class EditarDoctor
    {
        [Parameter] public int id { get; set; }
        [Inject] private IEspecialidadService serviciosEspecialidades { get; set; } = null!;

        private DoctorDTO? doctorOriginal;
        private ActualizarDoctorDTO doctorEditar = new();
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
            await CargarDoctor();
        }
        private async Task CargarDoctor()
        {
            var resultado = await DoctorService.ObtenerPorIdAsync(id);
            if (resultado.Exitoso && resultado.Datos != null)
            {
                doctorOriginal = resultado.Datos;
                // Mapear solo los campos editables al DTO de actualización
                doctorEditar.Id = doctorOriginal.Id;
                doctorEditar.Nombres = doctorOriginal.Nombres;
                doctorEditar.Apellidos = doctorOriginal.Apellidos;
                doctorEditar.Telefono = doctorOriginal.Telefono ?? string.Empty;
                doctorEditar.Email = doctorOriginal.Email ?? string.Empty;
                doctorEditar.HorarioAtencion = doctorOriginal.HorarioAtencion ?? string.Empty;
                doctorEditar.IdEspecialidad = doctorOriginal.IdEspecialidad;
                doctorEditar.FechaContratacion = doctorOriginal.FechaContratacion;
                doctorEditar.Descripcion = doctorOriginal.Descripcion ?? string.Empty;
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
            {
                await Toastr.MsgError("Error al actualizar: " + resultado.Mensaje);
            }
        }
        private void Cancelar() => Navigation.NavigateTo("/doctores");
    }
}
