using AutoMapper;
using FluentValidation;
using Aplicacion.Abstracciones;
using Aplicacion.DTOs.Citas;
using Aplicacion.Helpers;
using Aplicacion.Servicios.Interfaces;
using Dominio.Entidades.Citas;
using Dominio.Enumeraciones;

namespace Aplicacion.Servicios.Implementaciones
{
    public class CitaService : ICitaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CrearCitaDTO> _crearValidator;
        private readonly IValidator<ActualizarCitaDTO> _actualizarValidator;

        public CitaService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CrearCitaDTO> crearValidator,
            IValidator<ActualizarCitaDTO> actualizarValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        public async Task<ResultadoAccion<CitaDTO>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var cita = await _unitOfWork.Citas.ObtenerPorIdAsync(id);
                if (cita == null || cita.Eliminado)
                    return ResultadoAccion<CitaDTO>.Falla("Cita no encontrada.");

                var dto = _mapper.Map<CitaDTO>(cita);
                if (cita.Paciente != null)
                    dto.NombrePaciente = cita.Paciente.NombreCompleto;
                if (cita.Doctor != null)
                {
                    dto.NombreDoctor = cita.Doctor.NombreCompleto;
                    dto.NombreEspecialidad = cita.Doctor.Especialidad?.Nombre ?? string.Empty;
                }

                return ResultadoAccion<CitaDTO>.Exito(dto);
            }
            catch (Exception ex)
            {
                return ResultadoAccion<CitaDTO>.Falla($"Error al obtener la cita: {ex.Message}");
            }
        }

        public async Task<ResultadoAccion<IEnumerable<CitaDTO>>> ObtenerTodosAsync()
        {
            try
            {
                var citas = await _unitOfWork.Citas.BuscarAsync(c => !c.Eliminado);
                var dtos = _mapper.Map<IEnumerable<CitaDTO>>(citas);
                return ResultadoAccion<IEnumerable<CitaDTO>>.Exito(dtos);
            }
            catch (Exception ex)
            {
                return ResultadoAccion<IEnumerable<CitaDTO>>.Falla($"Error al obtener las citas: {ex.Message}");
            }
        }

        public async Task<ResultadoAccion<CitaDTO>> CrearAsync(CrearCitaDTO dto)
        {
            try
            {
                var validationResult = await _crearValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                    return ResultadoAccion<CitaDTO>.Falla("Datos inválidos",
                        validationResult.Errors.Select(e => e.ErrorMessage).ToList());

                var doctor = await _unitOfWork.Doctores.ObtenerPorIdAsync(dto.IdDoctor);
                if (doctor == null || !doctor.Activo)
                    return ResultadoAccion<CitaDTO>.Falla("El doctor seleccionado no está disponible.");

                if (await ExisteConflictoHorarioAsync(dto.IdDoctor, dto.FechaHora))
                    return ResultadoAccion<CitaDTO>.Falla("El doctor ya tiene una cita programada en ese horario.");

                var cita = _mapper.Map<Cita>(dto);
                cita.FechaDeCreacion = DateTime.UtcNow;
                cita.Activo = true;
                cita.Eliminado = false;

                await _unitOfWork.Citas.AgregarAsync(cita);
                await _unitOfWork.GuardarCambiosAsync();

                var citaCreada = await ObtenerPorIdAsync(cita.Id);
                return citaCreada;
            }
            catch (Exception ex)
            {
                return ResultadoAccion<CitaDTO>.Falla($"Error al crear la cita: {ex.Message}");
            }
        }

        public async Task<ResultadoAccion<CitaDTO>> ActualizarAsync(ActualizarCitaDTO dto)
        {
            try
            {
                var validationResult = await _actualizarValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                    return ResultadoAccion<CitaDTO>.Falla("Datos inválidos",
                        validationResult.Errors.Select(e => e.ErrorMessage).ToList());

                var citaExistente = await _unitOfWork.Citas.ObtenerPorIdAsync(dto.Id);
                if (citaExistente == null || citaExistente.Eliminado)
                    return ResultadoAccion<CitaDTO>.Falla("Cita no encontrada.");

                if (citaExistente.IdDoctor != dto.IdDoctor || citaExistente.FechaHora != dto.FechaHora)
                {
                    if (await ExisteConflictoHorarioAsync(dto.IdDoctor, dto.FechaHora, dto.Id))
                        return ResultadoAccion<CitaDTO>.Falla("El doctor ya tiene una cita programada en ese horario.");
                }

                _mapper.Map(dto, citaExistente);
                citaExistente.FechaDeModificacion = DateTime.UtcNow;

                if (dto.Estado == EstadoCita.Cancelada && citaExistente.Estado != EstadoCita.Cancelada)
                    citaExistente.FechaDeEliminacion = DateTime.UtcNow;
                else if (dto.Estado != EstadoCita.Cancelada)
                    citaExistente.FechaDeEliminacion = null;

                _unitOfWork.Citas.Actualizar(citaExistente);
                await _unitOfWork.GuardarCambiosAsync();

                return await ObtenerPorIdAsync(citaExistente.Id);
            }
            catch (Exception ex)
            {
                return ResultadoAccion<CitaDTO>.Falla($"Error al actualizar la cita: {ex.Message}");
            }
        }

        public async Task<ResultadoAccion> EliminarAsync(int id)
        {
            try
            {
                var cita = await _unitOfWork.Citas.ObtenerPorIdAsync(id);
                if (cita == null || cita.Eliminado)
                    return ResultadoAccion.Falla("Cita no encontrada.");

                cita.Eliminado = true;
                cita.FechaDeEliminacion = DateTime.UtcNow;
                cita.Activo = false;
                cita.Estado = EstadoCita.Cancelada;

                await _unitOfWork.Citas.Actualizar(cita);
                return ResultadoAccion.Exito("Cita eliminada correctamente.");
            }
            catch (Exception ex)
            {
                return ResultadoAccion.Falla($"Error al eliminar la cita: {ex.Message}");
            }
        }

        public async Task<bool> ExisteConflictoHorarioAsync(
            int idDoctor, DateTime fechaHora, int? excluirCitaId = null)
        {
            DateTime inicio = fechaHora.AddMinutes(-30);
            DateTime fin = fechaHora.AddMinutes(30);

            var citas = await _unitOfWork.Citas.BuscarAsync(c =>
                !c.Eliminado &&
                c.Estado != EstadoCita.Cancelada &&
                c.IdDoctor == idDoctor &&
                c.FechaHora >= inicio &&
                c.FechaHora <= fin &&
                (excluirCitaId == null || c.Id != excluirCitaId));

            return citas.Any();
        }
    }
}