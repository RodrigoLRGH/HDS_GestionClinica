using Microsoft.AspNetCore.Components;
using Aplicacion.DTOs.Pacientes;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Pacientes
{
    public partial class DetallePaciente : ComponentBase
    {
        [Inject] private IPacienteService ServiciosPaciente { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        [Parameter] public int id { get; set; }

        protected PacienteDTO? paciente;

        protected override async Task OnInitializedAsync()
        {
            var resultado = await ServiciosPaciente.ObtenerPorIdAsync(id);
            if (resultado.Exitoso && resultado.Datos != null)
            {
                paciente = resultado.Datos;
            }
            else
            {
                await Toastr.MsgError("Paciente no encontrado.");
                Navigation.NavigateTo("/pacientes");
            }
        }
        protected void Volver() => Navigation.NavigateTo("/pacientes");
        protected void Editar() => Navigation.NavigateTo($"/editar-paciente/{id}");
    }
}