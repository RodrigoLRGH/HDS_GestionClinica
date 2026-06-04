using AutoMapper;
using Aplicacion.DTOs.Pacientes;
using Dominio.Entidades.Pacientes;

namespace Aplicacion.Mapeos
{
    public class PacienteProfile : Profile
    {
        public PacienteProfile()
        {
            CreateMap<CrearPacienteDTO, Paciente>();

            CreateMap<ActualizarPacienteDTO, Paciente>()
                .ForMember(dest => dest.Nombres, opt => opt.MapFrom(src => src.Nombres))
                .ForMember(dest => dest.Apellidos, opt => opt.MapFrom(src => src.Apellidos))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
                // Campos protegidos
                .ForMember(dest => dest.NumeroDocumento, opt => opt.Ignore())
                .ForMember(dest => dest.TipoDocumento, opt => opt.Ignore())
                .ForMember(dest => dest.FechaNacimiento, opt => opt.Ignore())
                .ForMember(dest => dest.Genero, opt => opt.Ignore())
                .ForMember(dest => dest.GrupoSanguineo, opt => opt.Ignore());

            CreateMap<Paciente, PacienteDTO>().ReverseMap();
        }
    }
}