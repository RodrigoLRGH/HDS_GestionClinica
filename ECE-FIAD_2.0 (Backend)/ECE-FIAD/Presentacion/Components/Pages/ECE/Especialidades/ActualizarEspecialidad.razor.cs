using Aplicacion.DTOs.Especialidades;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
namespace Presentacion.Components.Pages.ECE.Especialidades
{
    partial class ActualizarEspecialidad
    {
        [Parameter] public int id { get; set; }
        private EspecialidadDTO? especialidadOriginal;
        private ActualizarEspecialidadDTO especialidadEditar = new();
        protected override async Task OnInitializedAsync()
        {
            await CargarEspecialidad();
        }
        private async Task CargarEspecialidad()
        {
            var resultado = await EspecialidadService.ObtenerPorIdAsync(id);
            if (resultado.Exitoso && resultado.Datos != null)
            {
                especialidadOriginal = resultado.Datos;
                // Mapear solo los campos editables al DTO de actualización
                especialidadEditar.Id = especialidadOriginal.Id;
                especialidadEditar.Nombre = especialidadOriginal.Nombre;
                especialidadEditar.Descripcion = especialidadOriginal.Descripcion;
            }
            else
            {
                await Toastr.MsgInformacion("Especialidad no encontrado o error al cargar.");
                Navigation.NavigateTo("/especialidades");
            }
        }
        private async Task GrabarEspecialidad()
        {
            var resultado = await EspecialidadService.ActualizarAsync(especialidadEditar);
            if (resultado.Exitoso)
            {
                await Toastr.MsgExito("Especialidad actualizada exitosamente.");
                Navigation.NavigateTo("/especialidades");
            }
            else
            {
                await Toastr.MsgError("Error al actualizar: " + resultado.Mensaje);
            }
        }
        private void Cancelar() => Navigation.NavigateTo("/especialidades");
    }
}