using Aplicacion.Abstracciones;
using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.Helpers;
using Aplicacion.Servicios.Interfaces;
using AutoMapper;
using Dominio.Entidades.HistoriasClinicas;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Servicios.Implementaciones
{
    internal class HistoriaClinicaService : IHistoriaClinicaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CrearHistoriaClinicaDTO> _crearValidacion;
        private readonly IValidator<ActualizarHistoriaClinicaDTO> _actualizarValidacion;

        public HistoriaClinicaService(IUnitOfWork unitOfWork, IMapper mapper,
            IValidator<CrearHistoriaClinicaDTO> crearValidacion,
            IValidator<ActualizarHistoriaClinicaDTO> actualizarValidacion)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _crearValidacion = crearValidacion;
            _actualizarValidacion = actualizarValidacion;
        }

        public async Task<ResultadoAccion<HistoriaClinicaDTO>> ObtenerPorIdAsync(int id)
        {
            var historiaClinica = await _unitOfWork.HistoriasClinicas.ObtenerPorIdAsync(id);
            if (historiaClinica == null)
                return ResultadoAccion<HistoriaClinicaDTO>.Falla("Historia clinica no encontrada");

            var dto = _mapper.Map<HistoriaClinicaDTO>(historiaClinica);
            return ResultadoAccion<HistoriaClinicaDTO>.Exito(dto);
        }

        public async Task<ResultadoAccion<IEnumerable<HistoriaClinicaDTO>>> ObtenerTodosAsync()
        {
            var historiaClinicas = await _unitOfWork.HistoriasClinicas.ObtenerTodosConRelacionesAsync();
            var dtos = _mapper.Map<IEnumerable<HistoriaClinicaDTO>>(historiaClinicas);
            return ResultadoAccion<IEnumerable<HistoriaClinicaDTO>>.Exito(dtos);
        }

        public async Task<ResultadoAccion<HistoriaClinicaDTO>> CrearAsync(CrearHistoriaClinicaDTO dto)
        {

            var validacion = await _crearValidacion.ValidateAsync(dto);
            if (!validacion.IsValid)
                return ResultadoAccion<HistoriaClinicaDTO>.Falla(
                    string.Join(" | ", validacion.Errors.Select(e => e.ErrorMessage)),
                    validacion.Errors.Select(e => e.ErrorMessage).ToList());

            var paciente = await _unitOfWork.Pacientes.ObtenerPorIdAsync(dto.IdPaciente);
            if (paciente == null)
                return ResultadoAccion<HistoriaClinicaDTO>.Falla("Paciente no existe");

            var tieneHistoria = await _unitOfWork.HistoriasClinicas.TieneHistoriaActivaAsync(dto.IdPaciente);
            if (tieneHistoria)
                return ResultadoAccion<HistoriaClinicaDTO>.Falla("El paciente ya tiene una historia clínica activa");

            var historia = _mapper.Map<HistoriaClinica>(dto);
            historia.FechaApertura = DateTime.UtcNow;

            await _unitOfWork.HistoriasClinicas.AgregarAsync(historia);
            await _unitOfWork.GuardarCambiosAsync();

            var historiaCreada = await _unitOfWork.HistoriasClinicas.ObtenerPorIdAsync(historia.Id);
            var dtoResultado = _mapper.Map<HistoriaClinicaDTO>(historiaCreada);
            return ResultadoAccion<HistoriaClinicaDTO>.Exito(dtoResultado, "Historia clinica creada exitosamente");
        }


        public async Task<ResultadoAccion<HistoriaClinicaDTO>> ActualizarAsync(ActualizarHistoriaClinicaDTO dto)
        {
            var validacion = await _actualizarValidacion.ValidateAsync(dto);
            if (!validacion.IsValid)
                return ResultadoAccion<HistoriaClinicaDTO>.Falla("Datos invalidos",
                    validacion.Errors.Select(e => e.ErrorMessage).ToList());

            var historiaClinica = await _unitOfWork.HistoriasClinicas.ObtenerPorIdAsync(dto.Id);
            if (historiaClinica == null)
                return ResultadoAccion<HistoriaClinicaDTO>.Falla("Historia clinica no encontrada");

            _mapper.Map(dto, historiaClinica);
            historiaClinica.FechaDeModificacion = DateTime.Now;


            _unitOfWork.HistoriasClinicas.Actualizar(historiaClinica);
            await _unitOfWork.GuardarCambiosAsync();

            var dtoResultado = _mapper.Map<HistoriaClinicaDTO>(historiaClinica);
            return ResultadoAccion<HistoriaClinicaDTO>.Exito(dtoResultado, "Historia clinica actualizada");
        }

        public async Task<ResultadoAccion> EliminarAsync(int id)
        {
            var historiaClinica = await _unitOfWork.HistoriasClinicas.ObtenerPorIdAsync(id);
            if (historiaClinica == null)
                return ResultadoAccion.Falla("Historia clinica no encontrada");

            historiaClinica.Activo = false;
            historiaClinica.Eliminado = true;
            historiaClinica.FechaDeEliminacion = DateTime.UtcNow;

            _unitOfWork.HistoriasClinicas.Actualizar(historiaClinica);
            await _unitOfWork.GuardarCambiosAsync();
            return ResultadoAccion.Exito("Historia clinica eliminada (borrado logico)");
        }
    }
}
