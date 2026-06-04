using AutoMapper;
using Aplicacion.DTOs.HistoriasClinicas;
using Dominio.Entidades.HistoriasClinicas;

namespace Aplicacion.Mapeos
{
    public class HistoriaClinicaProfile : Profile
    {
        public HistoriaClinicaProfile()
        {
            CreateMap<HistoriaClinica, HistoriaClinicaDTO>()
                .ForMember(dest => dest.PacienteNombre, opt => opt.Ignore());

            CreateMap<CrearHistoriaClinicaDTO, HistoriaClinica>() 
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeEliminacion, opt => opt.Ignore())
                .ForMember(dest => dest.Eliminado, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore())
                .ForMember(dest => dest.Paciente, opt => opt.Ignore())
                .ForMember(dest => dest.Evoluciones, opt => opt.Ignore());

            CreateMap<ActualizarHistoriaClinicaDTO, HistoriaClinica>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IdPaciente, opt => opt.Ignore())
                .ForMember(dest => dest.FechaApertura, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeEliminacion, opt => opt.Ignore())
                .ForMember(dest => dest.Eliminado, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore())
                .ForMember(dest => dest.Paciente, opt => opt.Ignore())
                .ForMember(dest => dest.Evoluciones, opt => opt.Ignore());


        }
    }
}