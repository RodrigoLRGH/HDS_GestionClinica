using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Helpers;
using Aplicacion.Servicios.Interfaces;
using Dominio.Entidades.Especialidades;
using Aplicacion.Abstracciones;
using AutoMapper;
using FluentValidation;
using Aplicacion.DTOs.Especialidades;

namespace Aplicacion.Servicios.Implementaciones
{
    public class EspecialidadService : IEspecialidadService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CrearEspecialidadDTO> _crearValidacion;
        private readonly IValidator<ActualizarEspecialidadDTO> _actualizarValidator;
        public EspecialidadService(IUnitOfWork unitOfWork, IMapper mapper,
        IValidator<CrearEspecialidadDTO> crearValidacion,
        IValidator<ActualizarEspecialidadDTO> actualizarValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _crearValidacion = crearValidacion;
            _actualizarValidator = actualizarValidator;
        }
        public async Task<ResultadoAccion<EspecialidadDTO>> ObtenerPorIdAsync(int id)
        {
            var Especialidad = await _unitOfWork.Especialidades.ObtenerPorIdAsync(id);
            if (Especialidad == null)
                return ResultadoAccion<EspecialidadDTO>.Falla("Especialidad no encontrado");
            var dto = _mapper.Map<EspecialidadDTO>(Especialidad);
            return ResultadoAccion<EspecialidadDTO>.Exito(dto);
        }
        public async Task<ResultadoAccion<IEnumerable<EspecialidadDTO>>> ObtenerTodosAsync()
        {
            var Especialidades = await _unitOfWork.Especialidades.ObtenerTodosAsync();
            var dtos = _mapper.Map<IEnumerable<EspecialidadDTO>>(Especialidades);
            return ResultadoAccion<IEnumerable<EspecialidadDTO>>.Exito(dtos);
        }
        public async Task<ResultadoAccion<EspecialidadDTO>> CrearAsync(CrearEspecialidadDTO dto)
        {
            // Validar
            var validacion = await _crearValidacion.ValidateAsync(dto);
            if (!validacion.IsValid)
            {
                return ResultadoAccion<EspecialidadDTO>.Falla("Datos inválidos",
                validacion.Errors.Select(e => e.ErrorMessage).ToList());
            }

            var existe = await _unitOfWork.Especialidades.ExisteAsync(e => e.Nombre.ToLower() == dto.Nombre.ToLower());

            if (existe)
                return ResultadoAccion<EspecialidadDTO>.Falla(
                    $"Ya existe una especialidad con el nombre '{dto.Nombre}'.");

            // Mapear y crear Especialidad
            var Especialidad = _mapper.Map<Especialidad>(dto);
            await _unitOfWork.Especialidades.AgregarAsync(Especialidad);
            await _unitOfWork.GuardarCambiosAsync();
            var EspecialidadCreado = await _unitOfWork.Especialidades.ObtenerPorIdAsync(Especialidad.Id);
            var dtoResultado = _mapper.Map<EspecialidadDTO>(EspecialidadCreado);
            return ResultadoAccion<EspecialidadDTO>.Exito(dtoResultado, "Especialidad creado exitosamente");
        }

        public async Task<ResultadoAccion<EspecialidadDTO>> ActualizarAsync(ActualizarEspecialidadDTO dto)
        {
            var validacion = await _actualizarValidator.ValidateAsync(dto);
            if (!validacion.IsValid)
                return ResultadoAccion<EspecialidadDTO>.Falla("Datos inválidos",
                validacion.Errors.Select(e => e.ErrorMessage).ToList());

            var Especialidad = await _unitOfWork.Especialidades.ObtenerPorIdAsync(dto.Id);
            if (Especialidad == null)
                return ResultadoAccion<EspecialidadDTO>.Falla("Especialidad no encontrado");

            var existe = await _unitOfWork.Especialidades.ExisteAsync(e => e.Nombre.ToLower() == dto.Nombre.ToLower());

            if (existe)
                return ResultadoAccion<EspecialidadDTO>.Falla(
                    $"Ya existe una especialidad con el nombre '{dto.Nombre}'.");

            _mapper.Map(dto, Especialidad);
            Especialidad.FechaDeModificacion = DateTime.UtcNow;
            _unitOfWork.Especialidades.Actualizar(Especialidad);
            await _unitOfWork.GuardarCambiosAsync();
            var dtoResultado = _mapper.Map<EspecialidadDTO>(Especialidad);
            return ResultadoAccion<EspecialidadDTO>.Exito(dtoResultado, "Especialidad actualizado");
        }
        public async Task<ResultadoAccion> EliminarAsync(int id)
        {
            var Especialidad = await _unitOfWork.Especialidades.ObtenerPorIdAsync(id);
            if (Especialidad == null)
                return ResultadoAccion.Falla("Especialidad no encontrado");
            // Borrado lógico
            if (Especialidad.Doctores.Any())
                return ResultadoAccion.Falla(
                    $"No se puede eliminar porque tiene {Especialidad.Doctores.Count} doctores(s) asociado(s).");
            Especialidad.Eliminado = true;
            Especialidad.FechaDeEliminacion = DateTime.UtcNow;
            Especialidad.Activo = false;
            _unitOfWork.Especialidades.Actualizar(Especialidad);
            await _unitOfWork.GuardarCambiosAsync();
            return ResultadoAccion.Exito("Especialidad eliminado (borrado lógico)");
        }
    }
}
