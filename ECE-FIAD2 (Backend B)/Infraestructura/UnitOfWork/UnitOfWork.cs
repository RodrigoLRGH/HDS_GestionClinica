using Aplicacion.Abstracciones;
using Infraestructura.Data;
using Infraestructura.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContextFactory<ContextoECE> _factory;

        public UnitOfWork(IDbContextFactory<ContextoECE> factory)
        {
            _factory = factory;
        }

        private IPacienteRepositorio? _pacienteRepositorio;
        public IPacienteRepositorio Pacientes =>
            _pacienteRepositorio ??= new PacienteRepositorio(_factory);

        private IDoctorRepositorio? _doctorRepositorio;
        public IDoctorRepositorio Doctores =>
            _doctorRepositorio ??= new DoctorRepositorio(_factory);

        private IEspecialidadRepositorio? _especialidadRepositorio;
        public IEspecialidadRepositorio Especialidades =>
            _especialidadRepositorio ??= new EspecialidadRepositorio(_factory);

        private ICitaRepositorio? _citaRepositorio;
        public ICitaRepositorio Citas =>
            _citaRepositorio ??= new CitaRepositorio(_factory);

        private IHistoriaClinicaRepositorio? _historiaRepositorio;
        public IHistoriaClinicaRepositorio HistoriasClinicas =>
            _historiaRepositorio ??= new HistoriaClinicaRepositorio(_factory);

        private IEvolucionRepositorio? _evolucionRepositorio;
        public IEvolucionRepositorio Evoluciones =>
            _evolucionRepositorio ??= new EvolucionRepositorio(_factory);

        public Task<int> GuardarCambiosAsync() => Task.FromResult(0);

        public void LimpiarChangeTracker() { }

        public void Dispose() { }
    }
}