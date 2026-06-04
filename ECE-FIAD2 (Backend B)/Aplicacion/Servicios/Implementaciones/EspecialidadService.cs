using AutoMapper;
using FluentValidation;
using Aplicacion.DTOs.Especialidades;
using Aplicacion.Helpers;
using Aplicacion.Servicios.Interfaces;
using Aplicacion.Abstracciones;
using Dominio.Entidades.Especialidades;

namespace Aplicacion.Servicios.Implementaciones
{
    public class EspecialidadService : IEspecialidadService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CrearEspecialidadDTO> _crearValidacion;
        private readonly IValidator<ActualizarEspecialidadDTO> _actualizarValidator;

        public EspecialidadService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
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
            var especialidad = await _unitOfWork.Especialidades.ObtenerPorIdAsync(id);
            if (especialidad == null)
                return ResultadoAccion<EspecialidadDTO>.Falla("Especialidad no encontrada.");

            var dto = _mapper.Map<EspecialidadDTO>(especialidad);
            dto.CantidadMedicos = await _unitOfWork.Especialidades
                .ContarDoctoresPorEspecialidadAsync(id);

            return ResultadoAccion<EspecialidadDTO>.Exito(dto);
        }

        public async Task<ResultadoAccion<IEnumerable<EspecialidadDTO>>> ObtenerTodosAsync()
        {
            var especialidades = await _unitOfWork.Especialidades.ObtenerTodosAsync();
            var conteos = await _unitOfWork.Especialidades.ObtenerConteosDoctoresAsync();

            var dtos = especialidades.Select(esp =>
            {
                var dto = _mapper.Map<EspecialidadDTO>(esp);
                dto.CantidadMedicos = conteos.TryGetValue(esp.Id, out var count) ? count : 0;
                return dto;
            });

            return ResultadoAccion<IEnumerable<EspecialidadDTO>>.Exito(dtos);
        }

        public async Task<ResultadoAccion<EspecialidadDTO>> CrearAsync(CrearEspecialidadDTO dto)
        {
            var validacion = await _crearValidacion.ValidateAsync(dto);
            if (!validacion.IsValid)
                return ResultadoAccion<EspecialidadDTO>.Falla("Datos inválidos",
                    validacion.Errors.Select(e => e.ErrorMessage).ToList());

            var existente = await _unitOfWork.Especialidades.ObtenerPorNombreAsync(dto.Nombre);

            if (existente is not null)
            {
                if (!existente.Eliminado)
                    return ResultadoAccion<EspecialidadDTO>.Falla("Ya existe una especialidad con ese nombre.");

                existente.Eliminado = false;
                existente.Activo = true;
                existente.Descripcion = dto.Descripcion;           
                existente.FechaDeModificacion = DateTime.UtcNow;
                existente.FechaDeEliminacion = default;
                _unitOfWork.Especialidades.Actualizar(existente);
                await _unitOfWork.GuardarCambiosAsync();

                var dtoReactivado = _mapper.Map<EspecialidadDTO>(existente);
                dtoReactivado.CantidadMedicos = await _unitOfWork.Especialidades 
                    .ContarDoctoresPorEspecialidadAsync(existente.Id);
                return ResultadoAccion<EspecialidadDTO>.Exito(dtoReactivado, "Especialidad reactivada exitosamente.");
            }

            try
            {
                var especialidad = _mapper.Map<Especialidad>(dto);
                await _unitOfWork.Especialidades.AgregarAsync(especialidad);
                await _unitOfWork.GuardarCambiosAsync();

                var especialidadCreada = await _unitOfWork.Especialidades.ObtenerPorIdAsync(especialidad.Id);
                var dtoResultado = _mapper.Map<EspecialidadDTO>(especialidadCreada);
                dtoResultado.CantidadMedicos = 0;                
                return ResultadoAccion<EspecialidadDTO>.Exito(dtoResultado, "Especialidad creada exitosamente.");
            }
            catch (Exception ex) when (
                ex.InnerException?.Message.Contains("duplicate key") == true ||
                ex.InnerException?.Message.Contains("IX_Especialidades_Nombre") == true ||
                ex.Message.Contains("duplicate key") == true)
            {
                return ResultadoAccion<EspecialidadDTO>.Falla("Ya existe una especialidad con ese nombre.");
            }
        }

        public async Task<ResultadoAccion<EspecialidadDTO>> ActualizarAsync(ActualizarEspecialidadDTO dto)
        {
            var validacion = await _actualizarValidator.ValidateAsync(dto);
            if (!validacion.IsValid)
                return ResultadoAccion<EspecialidadDTO>.Falla("Datos inválidos",
                    validacion.Errors.Select(e => e.ErrorMessage).ToList());

            var especialidad = await _unitOfWork.Especialidades.ObtenerPorIdAsync(dto.Id);
            if (especialidad == null || especialidad.Eliminado)
                return ResultadoAccion<EspecialidadDTO>.Falla("Especialidad no encontrada.");

            _mapper.Map(dto, especialidad);
            especialidad.FechaDeModificacion = DateTime.UtcNow;
            _unitOfWork.Especialidades.Actualizar(especialidad);
            int cambios = await _unitOfWork.GuardarCambiosAsync();

            if (cambios == 0)
                return ResultadoAccion<EspecialidadDTO>.Falla("No se guardaron cambios.");

            var dtoResultado = _mapper.Map<EspecialidadDTO>(especialidad);
            dtoResultado.CantidadMedicos = await _unitOfWork.Especialidades
                .ContarDoctoresPorEspecialidadAsync(especialidad.Id);
            return ResultadoAccion<EspecialidadDTO>.Exito(dtoResultado, "Especialidad actualizada.");
        }

        public async Task<ResultadoAccion> EliminarAsync(int id)
        {
            var especialidad = await _unitOfWork.Especialidades.ObtenerPorIdAsync(id);
            if (especialidad == null)
                return ResultadoAccion.Falla("Especialidad no encontrada.");

            especialidad.Eliminado = true;
            especialidad.FechaDeEliminacion = DateTime.UtcNow;
            especialidad.Activo = false;
            _unitOfWork.Especialidades.Actualizar(especialidad);
            await _unitOfWork.GuardarCambiosAsync();

            return ResultadoAccion.Exito("Especialidad eliminada (borrado lógico).");
        }
    }
}