using Microsoft.AspNetCore.Components;
using Aplicacion.DTOs.Evoluciones;
using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.DTOs.Doctores;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentacion.Components.Pages.ECE.Evoluciones
{
    public partial class CrearEvolucion : ComponentBase
    {
        [Inject] private IEvolucionService EvolucionService { get; set; } = null!;
        [Inject] private IHistoriaClinicaService HistoriaService { get; set; } = null!;
        [Inject] private IDoctorService DoctorService { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        private CrearEvolucionDTO nuevaEvolucion = new();
        private List<HistoriaClinicaDTO> historias = new();
        private List<DoctorDTO> doctores = new();
        private bool cargando = true;

        protected override async Task OnInitializedAsync()
        {
            await CargarHistorias();
            await CargarDoctores();
            cargando = false;
        }

        private async Task CargarHistorias()
        {
            var resultado = await HistoriaService.ObtenerTodosAsync();
            if (resultado.Exitoso && resultado.Datos != null)
                historias = resultado.Datos.Where(h => h.Activo).ToList();
        }

        private async Task CargarDoctores()
        {
            var resultado = await DoctorService.ObtenerTodosAsync();
            if (resultado.Exitoso && resultado.Datos != null)
                doctores = resultado.Datos.Where(d => d.Activo).ToList();
        }

        private async Task GrabarEvolucion()
        {
            var resultado = await EvolucionService.CrearAsync(nuevaEvolucion);
            if (resultado.Exitoso)
            {
                await Toastr.MsgExito("Evolución creada exitosamente.");
                Navigation.NavigateTo("/evoluciones");
            }
            else
            {
                await Toastr.MsgError("Error: " + resultado.Mensaje);
            }
        }

        private void Cancelar() => Navigation.NavigateTo("/evoluciones");
    }
}