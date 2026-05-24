using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.Pacientes;
using Aplicacion.Servicios.Implementaciones;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Presentacion.Components.Pages.ECE.HistoriasClinicas
{
    partial class EditarHistoriaClinica
    {
        [Parameter] public int id { get; set; }

        private HistoriaClinicaDTO? historiaClinicaOriginal;
        private ActualizarHistoriaClinicaDTO historiaClinicaEditar = new();
        protected List<PacienteDTO> pacientes = new();
        protected bool cargando = true;
        protected bool guardando = false;

        private void HandleFechaHoraChange(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value?.ToString(), out DateTime result))
            {
                historiaClinicaEditar.FechaApertura = result;
                historiaClinicaEditar.FechaApertura = result.ToUniversalTime();
            }
        }
        protected override async Task OnInitializedAsync()
        {
            var p = await pacienteService.ObtenerTodosAsync();

            if (p.Exitoso && p.Datos != null)
                pacientes = p.Datos.ToList();
            await CargarHistoriaClinica();
            cargando = false;
        }

        private async Task CargarHistoriaClinica()
        {
            var resultado = await serviciosHistoriaClinica.ObtenerPorIdAsync(id);

            if (resultado.Exitoso && resultado.Datos != null)
            {
                historiaClinicaOriginal = resultado.Datos;
                historiaClinicaEditar.Id = historiaClinicaOriginal.Id;
                historiaClinicaEditar.FechaApertura = historiaClinicaOriginal.FechaApertura;
                historiaClinicaEditar.Alergias = historiaClinicaOriginal.Alergias ?? string.Empty;
                historiaClinicaEditar.AntecedentesFamiliares = historiaClinicaOriginal.AntecedentesFamiliares ?? string.Empty;
                historiaClinicaEditar.AntecedentesPersonales = historiaClinicaOriginal.AntecedentesPersonales ?? string.Empty;
                historiaClinicaEditar.IdPaciente = historiaClinicaOriginal.IdPaciente;
                historiaClinicaEditar.Activo = historiaClinicaOriginal.Activo;
            }
            else
            {
                await Toastr.MsgError("Historia clinica no encontrada");
                Navigation.NavigateTo("/historias-clinicas");
            }
        }

        private async Task GrabarHistoriaClinica()
        {
            var resultado = await serviciosHistoriaClinica.ActualizarAsync(historiaClinicaEditar);

            if (resultado.Exitoso)
            {
                await Toastr.MsgExito("Historia clinica actualizada correctamente");
                Navigation.NavigateTo("/historias-clinicas");
            }
            else
            {
                await Toastr.MsgError(resultado.Mensaje);
            }
        }

        private void Cancelar() => Navigation.NavigateTo("/historias-clinicas");
    }
}
