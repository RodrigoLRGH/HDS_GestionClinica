using Aplicacion.DTOs.Citas;
using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.Pacientes;
using Aplicacion.Servicios.Interfaces;
using Dominio.Enumeraciones;
using Microsoft.AspNetCore.Components;
using Presentacion.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentacion.Components.Pages.ECE.Citas
{
    public partial class CrearCita : ComponentBase
    {
        [Inject] private ICitaService CitaService { get; set; } = null!;
        [Inject] private IPacienteService PacienteService { get; set; } = null!;
        [Inject] private IDoctorService DoctorService { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        private CrearCitaDTO citaDto = new()
        {
            FechaHora = DateTime.Now.AddHours(1), 
            Estado = EstadoCita.Pendiente
        };

        private List<PacienteDTO> pacientes = new();
        private List<DoctorDTO> doctores = new();
        private bool procesando = false;

        protected override async Task OnInitializedAsync()
        {
            await CargarPacientes();
            await CargarDoctores();
        }

        private async Task CargarPacientes()
        {
            var resultado = await PacienteService.ObtenerTodosAsync();
            if (resultado.Exitoso && resultado.Datos != null)
                pacientes = resultado.Datos.Where(p => p.Activo).ToList();
            else
                await Toastr.MsgError("Error al cargar pacientes: " + resultado.Mensaje);
        }

        private async Task CargarDoctores()
        {
            var resultado = await DoctorService.ObtenerTodosAsync();
            if (resultado.Exitoso && resultado.Datos != null)
                doctores = resultado.Datos.Where(d => d.Activo).ToList();
            else
                await Toastr.MsgError("Error al cargar doctores: " + resultado.Mensaje);
        }

        private async Task GrabarCita()
        {
            procesando = true;
            try
            {
                var resultado = await CitaService.CrearAsync(citaDto);
                if (resultado.Exitoso)
                {
                    await Toastr.MsgExito("Cita registrada correctamente.");
                    Navigation.NavigateTo("/citas");
                }
                else
                    await Toastr.MsgError("Error: " + resultado.Mensaje);
            }
            finally
            {
                procesando = false;
            }
        }

        private void Cancelar() => Navigation.NavigateTo("/citas");
    }




}