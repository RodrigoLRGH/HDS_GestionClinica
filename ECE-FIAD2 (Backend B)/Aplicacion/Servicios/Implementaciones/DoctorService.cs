using AutoMapper;
using FluentValidation;
using Aplicacion.DTOs.Doctores;
using Aplicacion.Helpers;
using Aplicacion.Servicios.Interfaces;
using Aplicacion.Abstracciones;
using Dominio.Entidades.Doctores;


namespace Aplicacion.Servicios.Implementaciones
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CrearDoctorDTO> _crearValidacion;
        private readonly IValidator<ActualizarDoctorDTO> _actualizarValidator;

        public DoctorService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CrearDoctorDTO> crearValidacion,
            IValidator<ActualizarDoctorDTO> actualizarValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _crearValidacion = crearValidacion;
            _actualizarValidator = actualizarValidator;
        }

        public async Task<ResultadoAccion<IEnumerable<DoctorDTO>>> ObtenerTodosAsync()
        {
            var doctores = await _unitOfWork.Doctores.ObtenerTodosAsync();
            var dtos = _mapper.Map<IEnumerable<DoctorDTO>>(doctores);
            return ResultadoAccion<IEnumerable<DoctorDTO>>.Exito(dtos);
        }

        public async Task<ResultadoAccion<DoctorDTO>> ObtenerPorIdAsync(int id)
        {
            var doctor = await _unitOfWork.Doctores.ObtenerPorIdAsync(id);
            if (doctor == null)
                return ResultadoAccion<DoctorDTO>.Falla("Doctor no encontrado");

            var dto = _mapper.Map<DoctorDTO>(doctor);
            return ResultadoAccion<DoctorDTO>.Exito(dto);
        }

        public async Task<ResultadoAccion<DoctorDTO>> CrearAsync(CrearDoctorDTO dto)
        {
            var validacion = await _crearValidacion.ValidateAsync(dto);
            if (!validacion.IsValid)
                return ResultadoAccion<DoctorDTO>.Falla("Datos inválidos",
                    validacion.Errors.Select(e => e.ErrorMessage).ToList());

            if (await ExisteEmailAsync(dto.Email))
                return ResultadoAccion<DoctorDTO>.Falla("Ya existe un doctor registrado con ese email");

            var especialidad = await _unitOfWork.Especialidades.ObtenerPorIdAsync(dto.IdEspecialidad);
            if (especialidad == null)
                return ResultadoAccion<DoctorDTO>.Falla("La especialidad seleccionada no existe");

            var doctor = _mapper.Map<Doctor>(dto);
            await _unitOfWork.Doctores.AgregarAsync(doctor);

            try
            {
                await _unitOfWork.GuardarCambiosAsync();
            }
            catch (Exception ex) when (
                ex.InnerException?.Message.Contains("IX_Doctores_Email") == true ||
                ex.InnerException?.Message.Contains("duplicate key") == true ||
                ex.Message.Contains("duplicate key") == true)
            {
                return ResultadoAccion<DoctorDTO>.Falla("Ya existe un doctor registrado con ese email");
            }

            var doctorCreado = await _unitOfWork.Doctores.ObtenerPorIdAsync(doctor.Id);
            var dtoResultado = _mapper.Map<DoctorDTO>(doctorCreado);
            return ResultadoAccion<DoctorDTO>.Exito(dtoResultado, "Doctor creado exitosamente");
        }



        public async Task<ResultadoAccion<DoctorDTO>> ActualizarAsync(ActualizarDoctorDTO dto)
        {
            var validacion = await _actualizarValidator.ValidateAsync(dto);
            if (!validacion.IsValid)
                return ResultadoAccion<DoctorDTO>.Falla("Datos inválidos",
                    validacion.Errors.Select(e => e.ErrorMessage).ToList());

            var doctor = await _unitOfWork.Doctores.ObtenerPorIdAsync(dto.Id);
            if (doctor == null)
                return ResultadoAccion<DoctorDTO>.Falla("Doctor no encontrado");

            if (await ExisteEmailAsync(dto.Email, dto.Id))
                return ResultadoAccion<DoctorDTO>.Falla("Ya existe otro doctor registrado con ese email");

            _mapper.Map(dto, doctor);
            doctor.FechaDeModificacion = DateTime.UtcNow;
            _unitOfWork.Doctores.Actualizar(doctor);
            await _unitOfWork.GuardarCambiosAsync();

            var dtoResultado = _mapper.Map<DoctorDTO>(doctor);
            return ResultadoAccion<DoctorDTO>.Exito(dtoResultado, "Doctor actualizado exitosamente");
        }

        public async Task<ResultadoAccion> EliminarAsync(int id)
        {
            var doctor = await _unitOfWork.Doctores.ObtenerPorIdAsync(id);
            if (doctor == null)
                return ResultadoAccion.Falla("Doctor no encontrado");

            if (await TieneAsociacionesAsync(id))
                return ResultadoAccion.Falla("El doctor tiene citas asociadas y no puede ser eliminado");

            doctor.Eliminado = true;
            doctor.FechaDeEliminacion = DateTime.UtcNow;
            doctor.Activo = false;
            _unitOfWork.Doctores.Actualizar(doctor);
            await _unitOfWork.GuardarCambiosAsync();

            return ResultadoAccion.Exito("Doctor eliminado correctamente");
        }

        public async Task<bool> ExisteEmailAsync(string email, int? excludeId = null)
        {
            var doctores = await _unitOfWork.Doctores.BuscarAsync(
                d => d.Email.ToLower() == email.ToLower() &&
                     (excludeId == null || d.Id != excludeId));
            return doctores.Any();
        }

        public async Task<bool> TieneAsociacionesAsync(int id)
        {
            var doctor = await _unitOfWork.Doctores.ObtenerPorIdAsync(id);
            return doctor?.Citas != null && doctor.Citas.Any();
        }
    }
}