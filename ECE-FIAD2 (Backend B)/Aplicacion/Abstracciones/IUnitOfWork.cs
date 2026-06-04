namespace Aplicacion.Abstracciones
{
    public interface IUnitOfWork : IDisposable
    {
        IPacienteRepositorio Pacientes { get; }
        IDoctorRepositorio Doctores { get; }
        IEspecialidadRepositorio Especialidades { get; }
        ICitaRepositorio Citas { get; }
        IHistoriaClinicaRepositorio HistoriasClinicas { get; }
        IEvolucionRepositorio Evoluciones { get; }
        Task<int> GuardarCambiosAsync();
        void LimpiarChangeTracker();
    }
}