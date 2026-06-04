using AutoMapper;
using Aplicacion.DTOs.Doctores;
using Dominio.Entidades.Doctores;

namespace Aplicacion.Mapeos
{
    public class DoctorProfile : Profile
    {
        public DoctorProfile()
        {
            CreateMap<Doctor, DoctorDTO>()
                .ForMember(dest => dest.NombreEspecialidad,
                    opt => opt.MapFrom(src => src.Especialidad != null
                        ? src.Especialidad.Nombre
                        : string.Empty))
                .ForMember(dest => dest.CantidadCitas,
                    opt => opt.MapFrom(src => src.Citas != null
                        ? src.Citas.Count
                        : 0));

            CreateMap<CrearDoctorDTO, Doctor>();

            CreateMap<ActualizarDoctorDTO, Doctor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())   
                .ForMember(dest => dest.Nombres, opt => opt.MapFrom(src => src.Nombres))
                .ForMember(dest => dest.Apellidos, opt => opt.MapFrom(src => src.Apellidos))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.HorarioAtencion, opt => opt.MapFrom(src => src.HorarioAtencion))
                .ForMember(dest => dest.IdEspecialidad, opt => opt.MapFrom(src => src.IdEspecialidad))
                .ForMember(dest => dest.FechaContratacion, opt => opt.MapFrom(src => src.FechaContratacion))
                // Campos protegidos
                .ForMember(dest => dest.Especialidad, opt => opt.Ignore())
                .ForMember(dest => dest.Citas, opt => opt.Ignore());
        }
    }
}