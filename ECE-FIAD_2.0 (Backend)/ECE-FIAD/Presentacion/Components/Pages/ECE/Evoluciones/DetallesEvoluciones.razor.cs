using Aplicacion.DTOs.Evoluciones;
using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Components;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Evoluciones
{
    partial class DetallesEvoluciones
    {
        [Inject] private IEvolucionService serviciosEvolucion { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;
        [Inject] private IHistoriaClinicaService historiaClinicaService { get; set; } = null!;
        [Inject] private IDoctorService doctorService { get; set; } = null!;
        protected List<HistoriaClinicaDTO> historiasClinicas = new();
        protected List<DoctorDTO> doctores = new();

        [Parameter] public int id { get; set; }

        private EvolucionDTO? evolucion;

        protected override async Task OnInitializedAsync()
        {
            var hc = await historiaClinicaService.ObtenerTodosAsync();
            var d = await doctorService.ObtenerTodosAsync();

            if (hc.Exitoso && hc.Datos != null)
                historiasClinicas = hc.Datos.ToList();

            if (d.Exitoso && d.Datos != null)
                doctores = d.Datos.ToList();

            await CargarEvolucion();
        }

        private async Task CargarEvolucion()
        {
            var resultado = await serviciosEvolucion.ObtenerPorIdAsync(id);

            if (resultado.Exitoso && resultado.Datos != null)
            {
                evolucion = resultado.Datos;
            }
            else
            {
                await Toastr.MsgError("Evolución no encontrada");
                Navigation.NavigateTo("/evoluciones");
            }
        }

        private void Volver() => Navigation.NavigateTo("/evoluciones");
        private void Editar() => Navigation.NavigateTo($"/editar-evolucion/{id}");
    }
}