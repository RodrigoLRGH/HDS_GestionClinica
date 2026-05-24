using Aplicacion.DTOs.Citas;
using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.Pacientes;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Components;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Citas
{
    public partial class DetallesCitas
    {
        [Parameter] public int id { get; set; }

        private CitaDTO? cita;
        private bool cargando = true;

        [Inject] private ICitaService CitaService { get; set; } = null!;
        [Inject] private IPacienteService pacienteService { get; set; } = null!;
        [Inject] private IDoctorService doctorService { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        protected List<PacienteDTO> pacientes = new();
        protected List<DoctorDTO> doctores = new();

        protected override async Task OnInitializedAsync()
        {
            var p = await pacienteService.ObtenerTodosAsync();
            var d = await doctorService.ObtenerTodosAsync();

            if (p.Exitoso && p.Datos != null)
                pacientes = p.Datos.ToList();
            if (d.Exitoso && d.Datos != null)
                doctores = d.Datos.ToList();

            var resultado = await CitaService.ObtenerPorIdAsync(id);
            if (resultado.Exitoso && resultado.Datos != null)
                cita = resultado.Datos;

            cargando = false;
        }

        private void Volver() => Navigation.NavigateTo("/citas");
        private void Editar() => Navigation.NavigateTo($"/editar-cita/{id}");
    }
}