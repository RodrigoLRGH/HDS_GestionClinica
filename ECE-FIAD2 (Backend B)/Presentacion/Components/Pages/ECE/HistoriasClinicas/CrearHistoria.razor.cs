using Microsoft.AspNetCore.Components;
using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.DTOs.Pacientes;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentacion.Components.Pages.ECE.HistoriasClinicas  
{
    public partial class CrearHistoria : ComponentBase
    {
        [Inject] private IHistoriaClinicaService HistoriaService { get; set; } = null!;
        [Inject] private IPacienteService PacienteService { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        protected CrearHistoriaClinicaDTO nuevaHistoria = new();
        private List<PacienteDTO> pacientes = new();
        private bool procesando = false;

        protected override async Task OnInitializedAsync()
        {
            var resultado = await PacienteService.ObtenerTodosAsync();
            if (resultado.Exitoso && resultado.Datos != null)
                pacientes = resultado.Datos.Where(p => p.Activo).ToList();
        }

        private async Task GrabarHistoria()
        {
            if (procesando) return;
            procesando = true;
            try
            {
                var resultado = await HistoriaService.CrearAsync(nuevaHistoria);
                if (resultado.Exitoso)
                {
                    await Toastr.MsgExito("Historia clínica creada.");
                    Navigation.NavigateTo("/historial");
                }
                else
                    await Toastr.MsgError("Error: " + resultado.Mensaje);
            }
            finally
            {
                procesando = false;
            }
        }

        private void Cancelar() => Navigation.NavigateTo("/historial");
    }
}