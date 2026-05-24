using Aplicacion.DTOs.Citas;
using Aplicacion.DTOs.Pacientes;
using Aplicacion.DTOs.Doctores;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Components;
using Presentacion.Servicios;
using Dominio.Enumeraciones;
using Aplicacion.Servicios.Implementaciones;

namespace Presentacion.Components.Pages.ECE.Citas
{
    public partial class CrearCita
    {
        [Inject] private ICitaService citaService { get; set; } = null!;
        [Inject] private IPacienteService pacienteService { get; set; } = null!;
        [Inject] private IDoctorService doctorService { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;

        protected CrearCitaDTO citaDto = new();
        protected List<PacienteDTO> pacientes = new();
        protected List<DoctorDTO> doctores = new();

        private void HandleFechaHoraChange(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value?.ToString(), out DateTime result))
            {
                citaDto.FechaHora = result;
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
        }

        protected async Task GrabarCita()
        {
            try
            {
                var resultado = await citaService.CrearAsync(citaDto);
                if (resultado.Exitoso)
                {
                    await Toastr.MsgExito("Cita creada correctamente");
                    Navigation.NavigateTo("/citas");
                }
                else
                {
                    await Toastr.MsgError(resultado.Mensaje ?? "Error al guardar");
                }
            }
            catch (Exception ex)
            {
                await Toastr.MsgError($"Error: {ex.Message}");
                Console.WriteLine(ex.ToString());
            }
        }

        protected void Cancelar()
        {
            Navigation.NavigateTo("/citas");
        }
    }
}