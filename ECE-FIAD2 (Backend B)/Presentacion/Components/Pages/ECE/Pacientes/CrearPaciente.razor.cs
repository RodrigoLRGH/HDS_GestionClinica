using Microsoft.AspNetCore.Components;
using Aplicacion.DTOs.Pacientes;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;
using Dominio.Enumeraciones;

namespace Presentacion.Components.Pages.ECE.Pacientes
{

    public partial class CrearPaciente : ComponentBase
    {
        [Inject] private IPacienteService ServiciosPaciente { get; set; } = null!;
        [Inject] private IToastrService ToastrService { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        protected CrearPacienteDTO pacienteDto { get; set; } = new()
        {
            TipoDocumento = TipoDeDocumento.Cedula,
            Genero = Genero.Masculino,
            GrupoSanguineo = GrupoSanguineo.APositivo
        };

        protected async Task GrabarPaciente()
        {
            var resultado = await ServiciosPaciente.CrearAsync(pacienteDto);

            if (resultado.Exitoso)
            {
                await ToastrService.MsgExito("Paciente creado exitosamente.");
                Navigation.NavigateTo("/pacientes");
            }
            else
            {
                await ToastrService.MsgError("Error al crear: " + resultado.Mensaje);
            }
        }
        protected void Cancelar() => Navigation.NavigateTo("/pacientes");
    }
}