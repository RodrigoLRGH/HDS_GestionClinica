using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Abstracciones
{
    public interface IUnitOfWork : IDisposable
    {
        IPacienteRepositorio Pacientes { get; }
        IEspecialidadRepositorio Especialidades { get; }
        IDoctorRepositorio Doctores { get; }
        ICitaRepositorio Citas { get; }
        IHistoriaClinicaRepositorio HistoriasClinicas { get; }
        IEvolucionRepositorio Evoluciones {  get; }
        // Aquí se agregarán más repositorios: IDoctorRepositorio, IEspecialidadRepositorio, etc.
        Task<int> GuardarCambiosAsync();
    }
}
