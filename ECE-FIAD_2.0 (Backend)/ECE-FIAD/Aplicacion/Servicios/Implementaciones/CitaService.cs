using Aplicacion.Abstracciones;
using Aplicacion.DTOs.Citas;
using Aplicacion.Helpers;
using Aplicacion.Servicios.Interfaces;
using AutoMapper;
using Dominio.Entidades.Citas;
using Dominio.Enumeraciones;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicacion.Servicios.Implementaciones
{
    public class CitaService : ICitaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CrearCitaDTO> _crearValidacion;
        private readonly IValidator<ActualizarCitaDTO> _actualizarValidacion;

        public CitaService(IUnitOfWork unitOfWork, IMapper mapper,
            IValidator<CrearCitaDTO> crearValidacion,
            IValidator<ActualizarCitaDTO> actualizarValidacion)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _crearValidacion = crearValidacion;
            _actualizarValidacion = actualizarValidacion;
        }

        public async Task<ResultadoAccion<CitaDTO>> ObtenerPorIdAsync(int id)
        {
            var cita = await _unitOfWork.Citas.ObtenerPorIdAsync(id);
            if (cita == null)
                return ResultadoAccion<CitaDTO>.Falla("Cita no encontrada");

            var dto = _mapper.Map<CitaDTO>(cita);
            return ResultadoAccion<CitaDTO>.Exito(dto);
        }

        public async Task<ResultadoAccion<IEnumerable<CitaDTO>>> ObtenerTodosAsync()
        {
            var citas = await _unitOfWork.Citas.ObtenerTodosConRelacionesAsync();
            var dtos = _mapper.Map<IEnumerable<CitaDTO>>(citas);
            return ResultadoAccion<IEnumerable<CitaDTO>>.Exito(dtos);
        }

        public async Task<ResultadoAccion<CitaDTO>> CrearAsync(CrearCitaDTO dto)
        {
            var validacion = await _crearValidacion.ValidateAsync(dto);
            if (!validacion.IsValid)
            {
                return ResultadoAccion<CitaDTO>.Falla("Datos invalidos",
                    validacion.Errors.Select(e => e.ErrorMessage).ToList());
            }

            var existe = await _unitOfWork.Citas.ExisteDisponibilidadAsync(dto.IdDoctor, dto.FechaHora, null);
            if (existe)
                return ResultadoAccion<CitaDTO>.Falla("El doctor ya tiene una cita en ese horario");

            var paciente = await _unitOfWork.Pacientes.ObtenerPorIdAsync(dto.IdPaciente);
            if (paciente == null)
                return ResultadoAccion<CitaDTO>.Falla("Paciente no existe");

            var doctor = await _unitOfWork.Doctores.ObtenerPorIdAsync(dto.IdDoctor);
            if (doctor == null)
                return ResultadoAccion<CitaDTO>.Falla("Doctor no existe");

            var cita = _mapper.Map<Cita>(dto);

            await _unitOfWork.Citas.AgregarAsync(cita);
            await _unitOfWork.GuardarCambiosAsync();

            var citaCreada = await _unitOfWork.Citas.ObtenerPorIdAsync(cita.Id);
            var dtoResultado = _mapper.Map<CitaDTO>(citaCreada);

            return ResultadoAccion<CitaDTO>.Exito(dtoResultado, "Cita creada exitosamente");
        }

        public async Task<ResultadoAccion<CitaDTO>> ActualizarAsync(ActualizarCitaDTO dto)
        {
            var validacion = await _actualizarValidacion.ValidateAsync(dto);
            if (!validacion.IsValid)
            {
                var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
                return ResultadoAccion<CitaDTO>.Falla(string.Join(" | ", errores), errores);
            }

            var existe = await _unitOfWork.Citas.ExisteDisponibilidadAsync(dto.IdDoctor, dto.FechaHora, dto.Id);
            if (existe)
                return ResultadoAccion<CitaDTO>.Falla("El doctor ya tiene una cita en ese horario");

            var cita = await _unitOfWork.Citas.ObtenerPorIdAsync(dto.Id);
            if (cita == null)
                return ResultadoAccion<CitaDTO>.Falla("Cita no encontrada");

            var estadoAnterior = cita.Estado;

            _mapper.Map(dto, cita);

            if (dto.Estado == EstadoCita.Cancelada && estadoAnterior != EstadoCita.Cancelada)
            {
                cita.FechaDeEliminacion = DateTime.UtcNow;
                cita.Eliminado = true;
            }
            else if (dto.Estado != EstadoCita.Cancelada && estadoAnterior == EstadoCita.Cancelada)
            {
                cita.FechaDeEliminacion = null;
                cita.Eliminado = false;
            }

            cita.FechaDeModificacion = DateTime.UtcNow;

            _unitOfWork.Citas.Actualizar(cita);
            await _unitOfWork.GuardarCambiosAsync();

            var dtoResultado = _mapper.Map<CitaDTO>(cita);
            return ResultadoAccion<CitaDTO>.Exito(dtoResultado, "Cita actualizada");
        }

        public async Task<ResultadoAccion> EliminarAsync(int id)
        {
            var cita = await _unitOfWork.Citas.ObtenerPorIdAsync(id);
            if (cita == null)
                return ResultadoAccion.Falla("Cita no encontrada");

            cita.Eliminado = true;
            cita.Activo = false;
            cita.FechaDeEliminacion = DateTime.UtcNow;

            _unitOfWork.Citas.Actualizar(cita);
            await _unitOfWork.GuardarCambiosAsync();

            return ResultadoAccion.Exito("Cita eliminada (borrado logico)");
        }

        public async Task<ResultadoAccion> CambiarEstadoAsync(int id, string accion)
        {
            var cita = await _unitOfWork.Citas.ObtenerPorIdAsync(id);
            if (cita == null)
                return ResultadoAccion.Falla("Cita no encontrada");

            switch (accion)
            {
                case "confirmar":
                    cita.Confirmar();
                    break;
                case "cancelar":
                    cita.Cancelar();
                    break;
                case "completar":
                    cita.Completar();
                    break;
                case "noasistio":
                    cita.RegistrarNoAsistencia();
                    break;
                default:
                    return ResultadoAccion.Falla("Accion invalida");
            }

            _unitOfWork.Citas.Actualizar(cita);
            await _unitOfWork.GuardarCambiosAsync();

            return ResultadoAccion.Exito("Estado actualizado");
        }
    }
}