using Microsoft.AspNetCore.Components;
using Aplicacion.DTOs.Citas;
using Aplicacion.DTOs.Pacientes;
using Aplicacion.DTOs.Doctores;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentacion.Components.Pages.ECE.Citas
{
    public partial class EditarCita : ComponentBase
    {
        [Inject] private ICitaService CitaService { get; set; } = null!;
        [Inject] private IPacienteService PacienteService { get; set; } = null!;
        [Inject] private IDoctorService DoctorService { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        [Parameter] public int id { get; set; }

        protected bool cargando = true;
        protected bool procesando = false;
        protected CitaDTO? citaOriginal;
        protected ActualizarCitaDTO citaEditor = new();
        protected List<PacienteDTO> pacientes = new();
        protected List<DoctorDTO> doctores = new();

        protected override async Task OnInitializedAsync()
        {
            await CargarPacientes();
            await CargarDoctores();
            await CargarCita();
            cargando = false;
        }

        private async Task CargarPacientes()
        {
            var result = await PacienteService.ObtenerTodosAsync();
            if (result.Exitoso && result.Datos != null)
                pacientes = result.Datos.Where(p => p.Activo).ToList();
        }

        private async Task CargarDoctores()
        {
            var result = await DoctorService.ObtenerTodosAsync();
            if (result.Exitoso && result.Datos != null)
                doctores = result.Datos.Where(d => d.Activo).ToList();
        }

        private async Task CargarCita()
        {
            var resultado = await CitaService.ObtenerPorIdAsync(id);
            if (resultado.Exitoso && resultado.Datos != null)
            {
                citaOriginal = resultado.Datos;
                citaEditor = new ActualizarCitaDTO
                {
                    Id = citaOriginal.Id,
                    IdPaciente = citaOriginal.IdPaciente,
                    IdDoctor = citaOriginal.IdDoctor,
                    FechaHora = citaOriginal.FechaHora,
                    Motivo = citaOriginal.Motivo,
                    Notas = citaOriginal.Notas,
                    Estado = citaOriginal.Estado
                };
            }
            else
            {
                await Toastr.MsgError("Cita no encontrada.");
                Navigation.NavigateTo("/citas");
            }
        }

        protected async Task GrabarCita()
        {
            procesando = true;
            try
            {
                var resultado = await CitaService.ActualizarAsync(citaEditor);
                if (resultado.Exitoso)
                {
                    await Toastr.MsgExito("Cita actualizada correctamente.");
                    Navigation.NavigateTo("/citas");
                }
                else
                {
                    await Toastr.MsgError("Error: " + resultado.Mensaje);
                }
            }
            finally
            {
                procesando = false;
            }
        }

        protected void Cancelar() => Navigation.NavigateTo("/citas");
    }
}