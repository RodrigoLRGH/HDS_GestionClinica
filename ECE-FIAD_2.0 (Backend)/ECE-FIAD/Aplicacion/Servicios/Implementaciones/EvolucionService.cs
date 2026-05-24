using Aplicacion.Abstracciones;
using Aplicacion.DTOs.Evoluciones;
using Aplicacion.Helpers;
using Aplicacion.Servicios.Interfaces;
using AutoMapper;
using Dominio.Entidades.Evoluciones;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Servicios.Implementaciones
{
    public class EvolucionService : IEvolucionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CrearEvolucionDTO> _crearValidacion;
        private readonly IValidator<ActualizarEvolucionDTO> _actualizarValidacion;

        public EvolucionService(IUnitOfWork unitOfWork, IMapper mapper,
            IValidator<CrearEvolucionDTO> crearValidacion,
            IValidator<ActualizarEvolucionDTO> actualizarValidacion)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _crearValidacion = crearValidacion;
            _actualizarValidacion = actualizarValidacion;
        }

        public async Task<ResultadoAccion<EvolucionDTO>> ObtenerPorIdAsync(int id)
        {
            var evolucion = await _unitOfWork.Evoluciones.ObtenerPorIdAsync(id);
            if (evolucion == null)
                return ResultadoAccion<EvolucionDTO>.Falla("Evolucion no encontrada");

            var dto = _mapper.Map<EvolucionDTO>(evolucion);
            return ResultadoAccion<EvolucionDTO>.Exito(dto);
        }

        public async Task<ResultadoAccion<IEnumerable<EvolucionDTO>>> ObtenerTodosAsync()
        {
            var evoluciones = await _unitOfWork.Evoluciones.ObtenerTodosConRelacionesAsync();
            var dtos = _mapper.Map<IEnumerable<EvolucionDTO>>(evoluciones);
            return ResultadoAccion<IEnumerable<EvolucionDTO>>.Exito(dtos);
        }

        public async Task<ResultadoAccion<EvolucionDTO>> CrearAsync(CrearEvolucionDTO dto)
        {
            var validacion = await _crearValidacion.ValidateAsync(dto);
            if (!validacion.IsValid)
            {
                return ResultadoAccion<EvolucionDTO>.Falla("Datos invalidos",
                    validacion.Errors.Select(e => e.ErrorMessage).ToList());
            }

            var doctor = await _unitOfWork.Doctores.ObtenerPorIdAsync(dto.IdDoctor);
            if (doctor == null)
                return ResultadoAccion<EvolucionDTO>.Falla("Doctor no existe");

            var evolucion = _mapper.Map<Evolucion>(dto);

            await _unitOfWork.Evoluciones.AgregarAsync(evolucion);
            await _unitOfWork.GuardarCambiosAsync();

            var evolucionCreada = await _unitOfWork.Evoluciones.ObtenerPorIdAsync(evolucion.Id);
            var dtoResultado = _mapper.Map<EvolucionDTO>(evolucionCreada);

            return ResultadoAccion<EvolucionDTO>.Exito(dtoResultado, "Evolucion creada exitosamente");
        }

        public async Task<ResultadoAccion<EvolucionDTO>> ActualizarAsync(ActualizarEvolucionDTO dto)
        {
            var validacion = await _actualizarValidacion.ValidateAsync(dto);
            if (!validacion.IsValid)
                return ResultadoAccion<EvolucionDTO>.Falla("Datos invalidos",
                    validacion.Errors.Select(e => e.ErrorMessage).ToList());
            var evolucion = await _unitOfWork.Evoluciones.ObtenerPorIdAsync(dto.Id);
            if (evolucion == null)
                return ResultadoAccion<EvolucionDTO>.Falla("Evolucion no encontrada");
            evolucion.FechaDeModificacion = DateTime.UtcNow;
            _mapper.Map(dto, evolucion);

            _unitOfWork.Evoluciones.Actualizar(evolucion);
            await _unitOfWork.GuardarCambiosAsync();

            var dtoResultado = _mapper.Map<EvolucionDTO>(evolucion);
            return ResultadoAccion<EvolucionDTO>.Exito(dtoResultado, "Evolucion actualizada");
        }

        public async Task<ResultadoAccion> EliminarAsync(int id)
        {
            var evolucion = await _unitOfWork.Evoluciones.ObtenerPorIdAsync(id);
            if (evolucion == null)
                return ResultadoAccion.Falla("Evolucion no encontrada");

            evolucion.Eliminado = true;
            evolucion.Activo = false;
            evolucion.FechaDeEliminacion = DateTime.UtcNow;

            _unitOfWork.Evoluciones.Actualizar(evolucion);
            await _unitOfWork.GuardarCambiosAsync();

            return ResultadoAccion.Exito("Evolucion eliminada (borrado logico)");
        }

        public async Task<ResultadoAccion<IEnumerable<EvolucionDTO>>> ObtenerPorHistoriaAsync(int idHistoria)
        {
            var evoluciones = await _unitOfWork.Evoluciones.ObtenerPorHistoriaAsync(idHistoria);
            var dtos = _mapper.Map<IEnumerable<EvolucionDTO>>(evoluciones);
            return ResultadoAccion<IEnumerable<EvolucionDTO>>.Exito(dtos);
        }

        public async Task<ResultadoAccion<IEnumerable<EvolucionDTO>>> ObtenerPorPacienteAsync(int pacienteId)
        {
            var evoluciones = await _unitOfWork.Evoluciones.ObtenerPorPacienteAsync(pacienteId);
            var dtos = _mapper.Map<IEnumerable<EvolucionDTO>>(evoluciones);
            return ResultadoAccion<IEnumerable<EvolucionDTO>>.Exito(dtos);
        }
    }
}
