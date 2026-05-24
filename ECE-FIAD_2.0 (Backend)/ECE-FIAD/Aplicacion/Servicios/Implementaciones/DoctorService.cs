using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using FluentValidation;
using Aplicacion.DTOs.Doctores;
using Aplicacion.Helpers;
using Aplicacion.Servicios.Interfaces;
using Dominio.Entidades.Doctores;
using Aplicacion.Abstracciones;
using Aplicacion.DTOs.Especialidades;

namespace Aplicacion.Servicios.Implementaciones
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CrearDoctorDTO> _crearValidacion;
        private readonly IValidator<ActualizarDoctorDTO> _actualizarValidacion;

        public DoctorService(IUnitOfWork unitOfWork, IMapper mapper,
            IValidator<CrearDoctorDTO> crearValidacion,
            IValidator<ActualizarDoctorDTO> actualizarValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _crearValidacion = crearValidacion;
            _actualizarValidacion = actualizarValidator;
        }

        public async Task<ResultadoAccion<DoctorDTO>> ObtenerPorIdAsync(int id)
        {
            var doctor = await _unitOfWork.Doctores.ObtenerPorIdAsync(id);
            if (doctor == null)
                return ResultadoAccion<DoctorDTO>.Falla("Doctor no encontrado");
            var dto = _mapper.Map<DoctorDTO>(doctor);
            return ResultadoAccion<DoctorDTO>.Exito(dto);
        }

        public async Task<ResultadoAccion<IEnumerable<DoctorDTO>>> ObtenerTodosAsync()
        {
            var doctores = await _unitOfWork.Doctores.ObtenerTodosAsync();
            var dtos = _mapper.Map<IEnumerable<DoctorDTO>>(doctores);
            return ResultadoAccion<IEnumerable<DoctorDTO>>.Exito(dtos);
        }

        public async Task<ResultadoAccion<IEnumerable<DoctorDTO>>> ObtenerTodosConEspecialidadAsync()
        {
            var doctores = await _unitOfWork.Doctores.ObtenerTodosConEspecialidadAsync();
            var dtos = _mapper.Map<IEnumerable<DoctorDTO>>(doctores);
            return ResultadoAccion<IEnumerable<DoctorDTO>>.Exito(dtos);
        }

        public async Task<ResultadoAccion<DoctorDTO>> CrearAsync(CrearDoctorDTO dto)
        {
            var validacion = await _crearValidacion.ValidateAsync(dto);
            if (!validacion.IsValid)
            {
                return ResultadoAccion<DoctorDTO>.Falla("Datos invalidos",
                    validacion.Errors.Select(e => e.ErrorMessage).ToList());
            }

            var existe = await _unitOfWork.Doctores.ExisteAsync(e => e.Email.ToLower() == dto.Email.ToLower());

            if (existe)
                return ResultadoAccion<DoctorDTO>.Falla(
                    $"Ya existe un doctor con el email '{dto.Email}'.");


            var doctor = _mapper.Map<Doctor>(dto);
            await _unitOfWork.Doctores.AgregarAsync(doctor);
            await _unitOfWork.GuardarCambiosAsync();
            var doctorCreado = await _unitOfWork.Doctores.ObtenerPorIdAsync(doctor.Id);
            var dtoResultado = _mapper.Map<DoctorDTO>(doctorCreado);
            return ResultadoAccion<DoctorDTO>.Exito(dtoResultado, "Doctor creado exitosamente");
        }

        public async Task<ResultadoAccion<DoctorDTO>> ActualizarAsync(ActualizarDoctorDTO dto)
        {
            var validacion = await _actualizarValidacion.ValidateAsync(dto);
            if (!validacion.IsValid)
                return ResultadoAccion<DoctorDTO>.Falla("Datos invalidos",
                    validacion.Errors.Select(e => e.ErrorMessage).ToList());
            var doctor = await _unitOfWork.Doctores.ObtenerPorIdAsync(dto.Id);
            if (doctor == null)
                return ResultadoAccion<DoctorDTO>.Falla("Doctor no encontrado");

            var existe = await _unitOfWork.Doctores.ExisteAsync(e => e.Email.ToLower() == dto.Email.ToLower() && e.Id != dto.Id);

            if (existe)
                return ResultadoAccion<DoctorDTO>.Falla(
                    $"Ya existe un doctor con el email '{dto.Email}'.");

            _mapper.Map(dto, doctor);
            doctor.FechaDeModificacion = DateTime.UtcNow;
            _unitOfWork.Doctores.Actualizar(doctor);
            await _unitOfWork.GuardarCambiosAsync();
            var dtoResultado = _mapper.Map<DoctorDTO>(doctor);
            return ResultadoAccion<DoctorDTO>.Exito(dtoResultado, "Doctor actualizado");
        }

        public async Task<ResultadoAccion> EliminarAsync(int id)
        {
            var doctor = await _unitOfWork.Doctores.ObtenerPorIdAsync(id);
            if (doctor == null)
                return ResultadoAccion.Falla("Doctor no encontrado");

            doctor.Eliminado = true;
            doctor.FechaDeEliminacion = DateTime.UtcNow;
            doctor.Activo = false;
            _unitOfWork.Doctores.Actualizar(doctor);
            await _unitOfWork.GuardarCambiosAsync();
            return ResultadoAccion.Exito("Doctor eliminado (borrado logico)");
        }
    }
}