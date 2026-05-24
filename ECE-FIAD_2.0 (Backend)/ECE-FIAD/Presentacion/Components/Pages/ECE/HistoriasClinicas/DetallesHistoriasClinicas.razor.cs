using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.DTOs.Pacientes;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.HistoriasClinicas
{
    partial class DetallesHistoriasClinicas
    {
        [Inject] private IToastrService Toastr { get; set; } = null!;
        protected List<PacienteDTO> pacientes = new();

        [Parameter] public int id { get; set; }

        private HistoriaClinicaDTO? historiaClinica;
        private bool cargando = true;

        protected override async Task OnInitializedAsync()
        {
            var p = await pacienteService.ObtenerTodosAsync();

            if (p.Exitoso && p.Datos != null)
                pacientes = p.Datos.ToList();

            await CargarHistoriaClinica();
            cargando = false;
        }

        private async Task CargarHistoriaClinica()
        {
            var resultado = await serviciosHistoriaClinica.ObtenerPorIdAsync(id);

            if (resultado.Exitoso && resultado.Datos != null)
            {
                historiaClinica = resultado.Datos;
            }
            else
            {
                await Toastr.MsgError("Historia clinica no encontrada");
                Navigation.NavigateTo("/historias-clinicas");
            }
        }


        private void Volver() => Navigation.NavigateTo("/historias-clinicas");

        private void Editar() => Navigation.NavigateTo($"/editar-historia/{id}");

        private void VerEvoluciones() => Navigation.NavigateTo($"/evoluciones-historia/{id}");
    }
}
