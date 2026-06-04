using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Aplicacion.Abstracciones;
using Aplicacion.DTOs.Evoluciones;
using Aplicacion.Helpers;
using Aplicacion.Servicios.Interfaces;
using Dominio.Entidades.Evoluciones;
using Dominio.Enumeraciones;

namespace Aplicacion.Servicios.Implementaciones
{
    public class EvolucionService : IEvolucionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CrearEvolucionDTO> _crearValidator;
        private readonly IValidator<ActualizarEvolucionDTO> _actualizarValidator;

        public EvolucionService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CrearEvolucionDTO> crearValidator,
            IValidator<ActualizarEvolucionDTO> actualizarValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        public async Task<ResultadoAccion<EvolucionDTO>> ObtenerPorIdAsync(int id)
        {
            var evolucion = await _unitOfWork.Evoluciones.ObtenerPorIdAsync(id);
            if (evolucion == null || evolucion.Eliminado)
                return ResultadoAccion<EvolucionDTO>.Falla("Evolución no encontrada.");

            var dto = _mapper.Map<EvolucionDTO>(evolucion);
            dto.PacienteNombre = evolucion.HistoriaClinica?.Paciente?.NombreCompleto ?? "N/A";
            dto.DoctorNombre = evolucion.Doctor?.NombreCompleto ?? "N/A";
            dto.EspecialidadDoctor = evolucion.Doctor?.Especialidad?.Nombre ?? "N/A";
            return ResultadoAccion<EvolucionDTO>.Exito(dto);
        }

        public async Task<ResultadoAccion<IEnumerable<EvolucionDTO>>> ObtenerTodosAsync()
        {
            var evoluciones = await _unitOfWork.Evoluciones.BuscarAsync(e => !e.Eliminado);

            var dtos = evoluciones.Select(e =>
            {
                var dto = _mapper.Map<EvolucionDTO>(e);
                dto.PacienteNombre = e.HistoriaClinica?.Paciente?.NombreCompleto ?? "N/A";
                dto.DoctorNombre = e.Doctor?.NombreCompleto ?? "N/A";
                dto.EspecialidadDoctor = e.Doctor?.Especialidad?.Nombre ?? "N/A";
                return dto;
            });

            return ResultadoAccion<IEnumerable<EvolucionDTO>>.Exito(dtos);
        }

        public async Task<ResultadoAccion<IEnumerable<EvolucionDTO>>> ObtenerPorHistoriaClinicaAsync(int idHistoriaClinica)
        {
            var evoluciones = await _unitOfWork.Evoluciones
                .ObtenerPorHistoriaClinicaAsync(idHistoriaClinica);

            var dtos = evoluciones.Select(e =>
            {
                var dto = _mapper.Map<EvolucionDTO>(e);
                dto.PacienteNombre = e.HistoriaClinica?.Paciente?.NombreCompleto ?? "N/A";
                dto.DoctorNombre = e.Doctor?.NombreCompleto ?? "N/A";
                dto.EspecialidadDoctor = e.Doctor?.Especialidad?.Nombre ?? "N/A";
                return dto;
            });

            return ResultadoAccion<IEnumerable<EvolucionDTO>>.Exito(dtos);
        }

        public async Task<ResultadoAccion<EvolucionDTO>> CrearAsync(CrearEvolucionDTO dto)
        {
            var validacion = await _crearValidator.ValidateAsync(dto);
            if (!validacion.IsValid)
                return ResultadoAccion<EvolucionDTO>.Falla("Datos inválidos",
                    validacion.Errors.Select(e => e.ErrorMessage).ToList());

            var evolucion = _mapper.Map<Evolucion>(dto);
            evolucion.Activo = true;    // ← faltaba
            evolucion.Eliminado = false;

            await _unitOfWork.Evoluciones.AgregarAsync(evolucion);
            await _unitOfWork.GuardarCambiosAsync();

            return await ObtenerPorIdAsync(evolucion.Id);
        }

        public async Task<ResultadoAccion<EvolucionDTO>> ActualizarAsync(ActualizarEvolucionDTO dto)
        {
            var validacion = await _actualizarValidator.ValidateAsync(dto);
            if (!validacion.IsValid)
                return ResultadoAccion<EvolucionDTO>.Falla("Datos inválidos",
                    validacion.Errors.Select(e => e.ErrorMessage).ToList());

            var evolucion = await _unitOfWork.Evoluciones.ObtenerPorIdAsync(dto.Id);
            if (evolucion == null || evolucion.Eliminado)
                return ResultadoAccion<EvolucionDTO>.Falla("Evolución no encontrada.");

            _mapper.Map(dto, evolucion);
            evolucion.FechaDeModificacion = DateTime.UtcNow;

            _unitOfWork.Evoluciones.Actualizar(evolucion);
            await _unitOfWork.GuardarCambiosAsync();

            return await ObtenerPorIdAsync(evolucion.Id);
        }

        public async Task<ResultadoAccion> EliminarAsync(int id)
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

        public async Task<ResultadoAccion<IEnumerable<EvolucionDTO>>> ObtenerPorPacienteAsync(int pacienteId)
        {
            var evoluciones = await _unitOfWork.Evoluciones.ObtenerPorPacienteAsync(pacienteId);

            var dtos = evoluciones.Select(e =>
            {
                var dto = _mapper.Map<EvolucionDTO>(e);
                dto.PacienteNombre = e.HistoriaClinica?.Paciente?.NombreCompleto ?? "N/A";
                dto.DoctorNombre = e.Doctor?.NombreCompleto ?? "N/A";
                dto.EspecialidadDoctor = e.Doctor?.Especialidad?.Nombre ?? "N/A";
                return dto;
            });

            return ResultadoAccion<IEnumerable<EvolucionDTO>>.Exito(dtos);
        }
    }
}