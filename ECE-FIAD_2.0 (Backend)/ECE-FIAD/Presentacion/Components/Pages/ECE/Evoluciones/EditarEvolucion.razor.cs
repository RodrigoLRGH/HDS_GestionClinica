using Aplicacion.DTOs.Evoluciones;
using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.Servicios.Implementaciones;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Presentacion.Components.Pages.ECE.Evoluciones
{
    public partial class EditarEvolucion
    {
        [Parameter] public int id { get; set; }

        private EvolucionDTO? evolucionOriginal;
        private ActualizarEvolucionDTO evolucionEditar = new();
        [Inject] private IHistoriaClinicaService historiaClinicaService { get; set; } = null!;
        [Inject] private IDoctorService doctorService { get; set; } = null!;
        protected List<HistoriaClinicaDTO> historiasClinicas = new();
        protected List<DoctorDTO> doctores = new();

        protected bool cargando = true;
        protected bool guardando = false;

        protected override async Task OnInitializedAsync()
        {
            var hc = await historiaClinicaService.ObtenerTodosAsync();
            var d = await doctorService.ObtenerTodosAsync();

            if (hc.Exitoso && hc.Datos != null)
                historiasClinicas = hc.Datos.ToList();

            if (d.Exitoso && d.Datos != null)
                doctores = d.Datos.ToList();
            await CargarEvolucion();
            cargando = false;
        }

        protected async Task CargarEvolucion()
        {
            var resultado = await EvolucionService.ObtenerPorIdAsync(id);

            if (resultado.Exitoso && resultado.Datos != null)
            {
                evolucionOriginal = resultado.Datos;
                evolucionEditar.Id = evolucionOriginal.Id;
                evolucionEditar.Fecha = evolucionOriginal.Fecha;
                evolucionEditar.Diagnostico = evolucionOriginal.Diagnostico ?? string.Empty;
                evolucionEditar.Tratamiento = evolucionOriginal.Tratamiento ?? string.Empty;
                evolucionEditar.Notas = evolucionOriginal.Notas ?? string.Empty;
                evolucionEditar.IdDoctor = evolucionOriginal.IdDoctor;
                evolucionEditar.IdHistoriaClinica = evolucionOriginal.IdHistoriaClinica;
            }
            else
            {
                await Toastr.MsgError("Evolucion no encontrada");
                Navigation.NavigateTo("/evoluciones");
            }
        }

        protected async Task GrabarEvolucion()
        {
            var resultado = await EvolucionService.ActualizarAsync(evolucionEditar);

            if (resultado.Exitoso)
            {
                await Toastr.MsgExito("Evolucion actualizada correctamente");
                Navigation.NavigateTo("/evoluciones");
            }
            else
            {
                await Toastr.MsgError(resultado.Mensaje);
            }
        }

        protected void Cancelar() => Navigation.NavigateTo("/evoluciones");

        protected void HandleFechaChange(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value?.ToString(), out DateTime result))
                evolucionEditar.Fecha = result.ToUniversalTime();
        }
    }
}