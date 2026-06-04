using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Aplicacion.Abstracciones;
using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.Helpers;
using Aplicacion.Servicios.Interfaces;
using Dominio.Entidades.HistoriasClinicas;

namespace Aplicacion.Servicios.Implementaciones
{
    public class HistoriaClinicaService : IHistoriaClinicaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CrearHistoriaClinicaDTO> _crearValidator;
        private readonly IValidator<ActualizarHistoriaClinicaDTO> _actualizarValidator;

        public HistoriaClinicaService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CrearHistoriaClinicaDTO> crearValidator,
            IValidator<ActualizarHistoriaClinicaDTO> actualizarValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        public async Task<ResultadoAccion<HistoriaClinicaDTO>> ObtenerPorIdAsync(int id)
        {
            var historia = await _unitOfWork.HistoriasClinicas.ObtenerPorIdAsync(id);
            if (historia == null || historia.Eliminado)
                return ResultadoAccion<HistoriaClinicaDTO>.Falla("Historia clínica no encontrada.");

            var dto = _mapper.Map<HistoriaClinicaDTO>(historia);
            dto.PacienteNombre = historia.Paciente?.NombreCompleto ?? "N/A";
            return ResultadoAccion<HistoriaClinicaDTO>.Exito(dto);
        }

        public async Task<ResultadoAccion<IEnumerable<HistoriaClinicaDTO>>> ObtenerTodosAsync()
        {
            var historias = await _unitOfWork.HistoriasClinicas.BuscarAsync(h => !h.Eliminado);

            var dtos = historias.Select(h =>
            {
                var dto = _mapper.Map<HistoriaClinicaDTO>(h);
                dto.PacienteNombre = h.Paciente?.NombreCompleto ?? "N/A"; 
                return dto;
            });

            return ResultadoAccion<IEnumerable<HistoriaClinicaDTO>>.Exito(dtos);
        }

        public async Task<ResultadoAccion<HistoriaClinicaDTO>> CrearAsync(CrearHistoriaClinicaDTO dto)
        {
            var validacion = await _crearValidator.ValidateAsync(dto);
            if (!validacion.IsValid)
                return ResultadoAccion<HistoriaClinicaDTO>.Falla("Datos inválidos",
                    validacion.Errors.Select(e => e.ErrorMessage).ToList());

            var historiaExistente = await _unitOfWork.HistoriasClinicas
                .ObtenerPorPacienteAsync(dto.IdPaciente);

            if (historiaExistente != null)
                return ResultadoAccion<HistoriaClinicaDTO>.Falla(
                    "El paciente ya tiene una historia clínica registrada.");

            try
            {
                var historia = _mapper.Map<HistoriaClinica>(dto);
                historia.Activo = true;
                historia.Eliminado = false;

                await _unitOfWork.HistoriasClinicas.AgregarAsync(historia);
                await _unitOfWork.GuardarCambiosAsync();

                return await ObtenerPorIdAsync(historia.Id);
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                if (inner.Contains("IX_HistoriasClinicas_IdPaciente") ||
                    inner.Contains("duplicate key") ||
                    inner.Contains("2601") ||
                    inner.Contains("2627"))
                {
                    return ResultadoAccion<HistoriaClinicaDTO>.Falla(
                        "El paciente ya tiene una historia clínica registrada.");
                }
                return ResultadoAccion<HistoriaClinicaDTO>.Falla($"Error al guardar: {inner}");
            }
        }


        public async Task<ResultadoAccion<HistoriaClinicaDTO>> ActualizarAsync(ActualizarHistoriaClinicaDTO dto)
        {
            var validacion = await _actualizarValidator.ValidateAsync(dto);
            if (!validacion.IsValid)
                return ResultadoAccion<HistoriaClinicaDTO>.Falla("Datos inválidos",
                    validacion.Errors.Select(e => e.ErrorMessage).ToList());

            var historia = await _unitOfWork.HistoriasClinicas.ObtenerPorIdAsync(dto.Id);
            if (historia == null || historia.Eliminado)
                return ResultadoAccion<HistoriaClinicaDTO>.Falla("Historia clínica no encontrada.");

            _mapper.Map(dto, historia);
            historia.FechaDeModificacion = DateTime.UtcNow;

            _unitOfWork.HistoriasClinicas.Actualizar(historia);
            await _unitOfWork.GuardarCambiosAsync();

            return await ObtenerPorIdAsync(historia.Id);
        }



        public async Task<ResultadoAccion> EliminarAsync(int id)
        {
            var historia = await _unitOfWork.HistoriasClinicas.ObtenerPorIdAsync(id);
            if (historia == null || historia.Eliminado)
                return ResultadoAccion.Falla("Historia clínica no encontrada.");

            historia.Eliminado = true;
            historia.FechaDeEliminacion = DateTime.UtcNow;
            historia.Activo = false;

            _unitOfWork.HistoriasClinicas.Actualizar(historia);
            await _unitOfWork.GuardarCambiosAsync();

            return ResultadoAccion.Exito("Historia clínica eliminada correctamente.");
        }

        public async Task<bool> ExisteHistoriaActivaPorPacienteAsync(int idPaciente, int? idExcluir = null)
        {
            var query = await _unitOfWork.HistoriasClinicas
                .BuscarAsync(h => h.IdPaciente == idPaciente && !h.Eliminado);
            if (idExcluir.HasValue)
                query = query.Where(h => h.Id != idExcluir.Value);
            return query.Any();
        }
    }
}