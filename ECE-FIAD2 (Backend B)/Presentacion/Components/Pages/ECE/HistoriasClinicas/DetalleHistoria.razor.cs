using Microsoft.AspNetCore.Components;
using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;
using System.Threading.Tasks;

namespace Presentacion.Components.Pages.ECE.HistoriasClinicas
{
    public partial class DetalleHistoria : ComponentBase
    {
        [Inject] private IHistoriaClinicaService HistoriaService { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        [Parameter] public int id { get; set; }

        private bool cargando = true;
        private HistoriaClinicaDTO? historia;

        protected override async Task OnInitializedAsync()
        {
            await CargarHistoria();
            cargando = false;
        }

        private async Task CargarHistoria()
        {
            var resultado = await HistoriaService.ObtenerPorIdAsync(id);
            if (resultado.Exitoso && resultado.Datos != null)
                historia = resultado.Datos;
            else
                await Toastr.MsgError("Historia no encontrada.");
        }

        private void Volver() => Navigation.NavigateTo("/historial");      // ← corregido
        private void Editar() => Navigation.NavigateTo($"/editar-historia/{id}");


    }
}