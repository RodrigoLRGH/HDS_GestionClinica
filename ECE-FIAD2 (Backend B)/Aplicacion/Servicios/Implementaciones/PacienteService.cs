using Aplicacion.Abstracciones;
using Aplicacion.DTOs.Pacientes;
using Aplicacion.Helpers;
using Aplicacion.Servicios.Interfaces;
using AutoMapper;
using Dominio.Entidades.HistoriasClinicas;
using Dominio.Entidades.Pacientes;
using FluentValidation;

namespace Aplicacion.Servicios.Implementaciones
{
    public class PacienteService : IPacienteService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IValidator<CrearPacienteDTO> _crearValidator;
        private readonly IValidator<ActualizarPacienteDTO> _actualizarValidator;

        public PacienteService(IUnitOfWork uow, IMapper mapper,
            IValidator<CrearPacienteDTO> crearValidator,
            IValidator<ActualizarPacienteDTO> actualizarValidator)
        {
            _uow = uow;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        public async Task<ResultadoAccion<PacienteDTO>> ObtenerPorIdAsync(int id)
        {
            var paciente = await _uow.Pacientes.ObtenerPorIdAsync(id);
            if (paciente == null) return ResultadoAccion<PacienteDTO>.Falla("Paciente no encontrado");
            return ResultadoAccion<PacienteDTO>.Exito(_mapper.Map<PacienteDTO>(paciente));
        }

        public async Task<ResultadoAccion<IEnumerable<PacienteDTO>>> ObtenerTodosAsync()
        {
            var pacientes = await _uow.Pacientes.ObtenerTodosAsync();
            return ResultadoAccion<IEnumerable<PacienteDTO>>.Exito(_mapper.Map<IEnumerable<PacienteDTO>>(pacientes));
        }

        public async Task<ResultadoAccion<PacienteDTO>> CrearAsync(CrearPacienteDTO dto)
        {
            var validacion = await _crearValidator.ValidateAsync(dto);
            if (!validacion.IsValid)
                return ResultadoAccion<PacienteDTO>.Falla("Datos inválidos", validacion.Errors.Select(e => e.ErrorMessage).ToList());

            if (await ExistePorDocumentoAsync(dto.NumeroDocumento))
                return ResultadoAccion<PacienteDTO>.Falla("Ya existe un paciente con ese número de documento");

            var paciente = _mapper.Map<Paciente>(dto);
            await _uow.Pacientes.AgregarAsync(paciente);
            await _uow.GuardarCambiosAsync();

            var historia = new HistoriaClinica
            {
                IdPaciente = paciente.Id,
                FechaApertura = DateTime.Now,
                Activo = true,        
                Eliminado = false  
            };


            await _uow.HistoriasClinicas.AgregarAsync(historia);
            await _uow.GuardarCambiosAsync();

            return ResultadoAccion<PacienteDTO>.Exito(_mapper.Map<PacienteDTO>(paciente), "Paciente creado exitosamente");
        }

        public async Task<ResultadoAccion<PacienteDTO>> ActualizarAsync(ActualizarPacienteDTO dto)
        {
            var validacion = await _actualizarValidator.ValidateAsync(dto);
            if (!validacion.IsValid)
                return ResultadoAccion<PacienteDTO>.Falla("Datos inválidos", validacion.Errors.Select(e => e.ErrorMessage).ToList());

            var paciente = await _uow.Pacientes.ObtenerPorIdAsync(dto.Id);
            if (paciente == null) return ResultadoAccion<PacienteDTO>.Falla("Paciente no encontrado");

            _mapper.Map(dto, paciente);
            paciente.FechaDeModificacion = DateTime.UtcNow;
            _uow.Pacientes.Actualizar(paciente);
            await _uow.GuardarCambiosAsync();

            return ResultadoAccion<PacienteDTO>.Exito(_mapper.Map<PacienteDTO>(paciente), "Paciente actualizado");
        }

        public async Task<ResultadoAccion> EliminarAsync(int id)
        {
            var paciente = await _uow.Pacientes.ObtenerPorIdAsync(id);
            if (paciente == null) return ResultadoAccion.Falla("Paciente no encontrado");

            paciente.Eliminado = true;
            paciente.FechaDeEliminacion = DateTime.UtcNow;
            paciente.Activo = false;
            _uow.Pacientes.Actualizar(paciente);
            await _uow.GuardarCambiosAsync();

            return ResultadoAccion.Exito("Paciente eliminado (borrado lógico)");
        }

        public async Task<bool> ExistePorDocumentoAsync(string numeroDocumento) =>
            (await _uow.Pacientes.BuscarAsync(p => p.NumeroDocumento == numeroDocumento)).Any();
    }
}