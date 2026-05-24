using Aplicacion.DTOs.Citas;
using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.Pacientes;
using Aplicacion.Servicios.Implementaciones;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Dominio.Enumeraciones;

namespace Presentacion.Components.Pages.ECE.Citas
{
    public partial class EditarCita
    {
        [Parameter] public int id { get; set; }

        private CitaDTO? citaOriginal;
        private ActualizarCitaDTO citaEditar = new();
        [Inject] private IPacienteService pacienteService { get; set; } = null!;
        [Inject] private IDoctorService doctorService { get; set; } = null!;
        protected List<PacienteDTO> pacientes = new();
        protected List<DoctorDTO> doctores = new();

        private bool cargando = true;
        private bool guardando = false;

        private void HandleFechaHoraChange(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value?.ToString(), out DateTime result))
            {
                citaEditar.FechaHora = result;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            var p = await pacienteService.ObtenerTodosAsync();
            var d = await doctorService.ObtenerTodosAsync();

            if (p.Exitoso && p.Datos != null)
                pacientes = p.Datos.ToList();
            if (d.Exitoso && d.Datos != null)
                doctores = d.Datos.ToList();

            await CargarCita();

            cargando = false;
        }

        private async Task CargarCita()
        {
            var resultado = await CitaService.ObtenerPorIdAsync(id);

            if (resultado.Exitoso && resultado.Datos != null)
            {
                citaOriginal = resultado.Datos;
                citaEditar.Id = citaOriginal.Id;
                citaEditar.FechaHora = citaOriginal.FechaHora;
                citaEditar.Motivo = citaOriginal.Motivo ?? string.Empty;
                citaEditar.Notas = citaOriginal.Notas ?? string.Empty;
                citaEditar.IdDoctor = citaOriginal.IdDoctor;
                citaEditar.IdPaciente = citaOriginal.IdPaciente;
                citaEditar.Estado = citaOriginal.Estado;
            }
            else
            {
                await Toastr.MsgError("Cita no encontrada");
                Navigation.NavigateTo("/citas");
            }
        }

        private async Task GrabarCita()
        {
            guardando = true;
            try
            {
                var resultado = await CitaService.ActualizarAsync(citaEditar);

                if (resultado.Exitoso)
                {
                    await Toastr.MsgExito("Cita actualizada correctamente");
                    Navigation.NavigateTo("/citas");
                }
                else
                {
                    await Toastr.MsgError(resultado.Mensaje);
                }
            }
            catch (Exception ex)
            {
                {
                    await Toastr.MsgError($"Error: {ex.Message}");
                    Console.WriteLine(ex.ToString());
                }
                guardando = false;
            }
        }

        private void Cancelar() => Navigation.NavigateTo("/citas");
    }
}