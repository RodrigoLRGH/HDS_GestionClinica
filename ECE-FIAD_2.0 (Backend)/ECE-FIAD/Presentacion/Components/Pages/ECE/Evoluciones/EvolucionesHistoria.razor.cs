using Aplicacion.DTOs.Evoluciones;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Presentacion.Components.Pages.ECE.Evoluciones
{
    partial class EvolucionesHistoria
    {
        [Parameter] public int idHistoria { get; set; }

        private List<EvolucionDTO> evoluciones = new();
        private string nombrePaciente = string.Empty;
        private bool cargando = true;

        [Inject] private IEvolucionService serviciosEvolucion { get; set; } = null!;
        [Inject] private IHistoriaClinicaService serviciosHistoriaClinica { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            var historia = await serviciosHistoriaClinica.ObtenerPorIdAsync(idHistoria);
            if (historia.Exitoso && historia.Datos != null)
                nombrePaciente = historia.Datos.Paciente?.Nombres ?? "Paciente";

            var resultado = await serviciosEvolucion.ObtenerPorHistoriaAsync(idHistoria);
            if (resultado.Exitoso && resultado.Datos != null)
                evoluciones = resultado.Datos.ToList();
            

            cargando = false;
        }

        private string Truncar(string? texto, int longitud = 40)
        {
            if (string.IsNullOrEmpty(texto)) return "—";
            return texto.Length > longitud ? texto.Substring(0, longitud) + "..." : texto;
        }

        private void VerDetalles(int id) => Navigation.NavigateTo($"/detalles-evolucion/{id}");
        private void Editar(int id) => Navigation.NavigateTo($"/editar-evolucion/{id}");
        private void NavegarACrear() => Navigation.NavigateTo($"/crear-evolucion/{idHistoria}");
        private void Volver() => Navigation.NavigateTo($"/detalles-historia/{idHistoria}");
    }
}