using AutoMapper;
using Aplicacion.DTOs.Especialidades;
using Dominio.Entidades.Especialidades;

namespace Aplicacion.Mapeos
{
    public class EspecialidadProfile : Profile
    {
        public EspecialidadProfile()
        {
            CreateMap<Especialidad, EspecialidadDTO>()
                .ForMember(dest => dest.CantidadMedicos, opt => opt.Ignore());

            CreateMap<CrearEspecialidadDTO, Especialidad>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeEliminacion, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore())
                .ForMember(dest => dest.Eliminado, opt => opt.Ignore())
                .ForMember(dest => dest.Doctores, opt => opt.Ignore());

            CreateMap<ActualizarEspecialidadDTO, Especialidad>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeEliminacion, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore())
                .ForMember(dest => dest.Eliminado, opt => opt.Ignore())
                .ForMember(dest => dest.Doctores, opt => opt.Ignore());
        }
    }
}