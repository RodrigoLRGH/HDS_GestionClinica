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

    public partial class EditarHistoria : ComponentBase
    {
        [Inject] private IHistoriaClinicaService HistoriaService { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;          // ← PacienteService eliminado
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        [Parameter] public int id { get; set; }

        private bool cargando = true;
        private ActualizarHistoriaClinicaDTO historiaEditar = new();
        private HistoriaClinicaDTO? historiaOriginal;
        // private List<PacienteDTO> pacientes = new(); ← eliminar, no se usa

        protected override async Task OnInitializedAsync()
        {
            // var resPacientes = ... ← eliminar, no se necesita
            await CargarHistoria();
            cargando = false;
        }

        private async Task CargarHistoria()
        {
            var resultado = await HistoriaService.ObtenerPorIdAsync(id);
            if (resultado.Exitoso && resultado.Datos != null)
            {
                historiaOriginal = resultado.Datos;
                historiaEditar = new ActualizarHistoriaClinicaDTO
                {
                    Id = historiaOriginal.Id,
                    Alergias = historiaOriginal.Alergias,
                    AntecedentesFamiliares = historiaOriginal.AntecedentesFamiliares,
                    AntecedentesPersonales = historiaOriginal.AntecedentesPersonales
                };
            }
            else
            {
                await Toastr.MsgError("Historia no encontrada.");
                Navigation.NavigateTo("/historial"); // ← corregido
            }
        }

        private async Task GrabarHistoria()
        {
            var resultado = await HistoriaService.ActualizarAsync(historiaEditar);
            if (resultado.Exitoso)
            {
                await Toastr.MsgExito("Historia actualizada.");
                Navigation.NavigateTo("/historial");
            }
            else
                await Toastr.MsgError("Error: " + resultado.Mensaje);
        }

        private void Cancelar() => Navigation.NavigateTo("/historial");
    }


}