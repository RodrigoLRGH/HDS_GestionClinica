using Aplicacion.DTOs.Evoluciones;
using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Components;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Evoluciones
{
    public partial class CrearEvolucion
    {
        [Inject] private IEvolucionService evolucionService { get; set; } = null!;
        [Inject] private IDoctorService doctorService { get; set; } = null!;
        [Inject] private IHistoriaClinicaService historiaClinicaService { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;

        protected CrearEvolucionDTO evolucionDto = new();
        protected List<DoctorDTO> doctores = new();
        protected List<HistoriaClinicaDTO> historiasClinicas = new();

        protected override async Task OnInitializedAsync()
        {
            var d = await doctorService.ObtenerTodosConEspecialidadAsync();
            var hc = await historiaClinicaService.ObtenerTodosAsync();

            if (d.Exitoso && d.Datos != null)
                doctores = d.Datos.ToList();

            if (hc.Exitoso && hc.Datos != null)
                historiasClinicas = hc.Datos.ToList();
        }

        protected async Task GrabarEvolucion()
        {
            var resultado = await evolucionService.CrearAsync(evolucionDto);

            if (resultado.Exitoso)
            {
                await Toastr.MsgExito("Evolucion creada correctamente");
                Navigation.NavigateTo("/evoluciones");
            }
            else
            {
                await Toastr.MsgError(resultado.Mensaje);
            }
        }

        protected void Cancelar()
        {
            Navigation.NavigateTo("/evoluciones");
        }

        private void HandleFechaChange(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value?.ToString(), out DateTime result))
                evolucionDto.Fecha = result.ToUniversalTime();
        }
    }
}