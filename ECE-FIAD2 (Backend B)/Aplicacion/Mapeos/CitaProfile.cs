using AutoMapper;
using Aplicacion.DTOs.Citas;
using Dominio.Entidades.Citas;

namespace Aplicacion.Mapeos
{
    public class CitaProfile : Profile
    {
        public CitaProfile()
        {
            CreateMap<Cita, CitaDTO>()
                .ForMember(dest => dest.NombrePaciente,
                    opt => opt.MapFrom(src => src.Paciente != null ? src.Paciente.NombreCompleto : "N/A"))
                .ForMember(dest => dest.NombreDoctor,
                    opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.NombreCompleto : "N/A"))
                .ForMember(dest => dest.NombreEspecialidad,
                    opt => opt.MapFrom(src => src.Doctor != null && src.Doctor.Especialidad != null
                        ? src.Doctor.Especialidad.Nombre : "N/A"))
                .ForMember(dest => dest.FechaCreacion,
                    opt => opt.MapFrom(src => src.FechaDeCreacion))
                .ForMember(dest => dest.FechaModificacion,
                    opt => opt.MapFrom(src => src.FechaDeModificacion))
                .ForMember(dest => dest.FechaEliminacion,
                    opt => opt.MapFrom(src => src.FechaDeEliminacion));




            CreateMap<CrearCitaDTO, Cita>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeEliminacion, opt => opt.Ignore())
                .ForMember(dest => dest.Eliminado, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore())
                .ForMember(dest => dest.Paciente, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore());
            CreateMap<ActualizarCitaDTO, Cita>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())  
                .ForMember(dest => dest.FechaDeCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeEliminacion, opt => opt.Ignore())
                .ForMember(dest => dest.Eliminado, opt => opt.Ignore())  
                .ForMember(dest => dest.Activo, opt => opt.Ignore())  
                .ForMember(dest => dest.Paciente, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore());
        }
    }
}