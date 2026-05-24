using Aplicacion.DTOs.Citas;
using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.DTOs.Pacientes;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Components;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.HistoriasClinicas
{
    partial class CrearHistoriaClinica
    {
        [Inject] private IHistoriaClinicaService historiaClinicaService { get; set; } = null!;
        [Inject] private IPacienteService pacienteService { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;

        protected CrearHistoriaClinicaDTO historiaClinicaDto = new();
        protected List<PacienteDTO> pacientes = new();

        private void HandleFechaHoraChange(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value?.ToString(), out DateTime result))
                historiaClinicaDto.FechaApertura = result.ToUniversalTime();
        }
        protected override async Task OnInitializedAsync()
        {
            var p = await pacienteService.ObtenerTodosAsync();

            if (p.Exitoso && p.Datos != null)
                pacientes = p.Datos.ToList();
        }

        protected async Task GrabarHistoriaClinica()
        {
            var resultado = await historiaClinicaService.CrearAsync(historiaClinicaDto);

            if (resultado.Exitoso)
            {
                await Toastr.MsgExito("Historia clinica creada correctamente");
                Navigation.NavigateTo("/historias-clinicas");
            }
            else
            {
                await Toastr.MsgError(resultado.Mensaje ?? "Error desconocido");
            }
        }

        protected void Cancelar()
        {
            Navigation.NavigateTo("/historias-clinicas");
        }
    }
}
