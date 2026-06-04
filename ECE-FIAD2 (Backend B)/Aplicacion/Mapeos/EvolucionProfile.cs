using AutoMapper;
using Aplicacion.DTOs.Evoluciones;
using Dominio.Entidades.Evoluciones;

namespace Aplicacion.Mapeos
{
    public class EvolucionProfile : Profile
    {
        public EvolucionProfile()
        {
            CreateMap<Evolucion, EvolucionDTO>()
                .ForMember(dest => dest.PacienteNombre, opt => opt.Ignore())
                .ForMember(dest => dest.DoctorNombre, opt => opt.Ignore())
                .ForMember(dest => dest.EspecialidadDoctor, opt => opt.Ignore());

            CreateMap<CrearEvolucionDTO, Evolucion>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeEliminacion, opt => opt.Ignore())
                .ForMember(dest => dest.Eliminado, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore()) 
                .ForMember(dest => dest.HistoriaClinica, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore());

            CreateMap<ActualizarEvolucionDTO, Evolucion>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IdHistoriaClinica, opt => opt.Ignore()) 
                .ForMember(dest => dest.IdDoctor, opt => opt.Ignore()) 
                .ForMember(dest => dest.Fecha, opt => opt.Ignore()) 
                .ForMember(dest => dest.FechaDeCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeEliminacion, opt => opt.Ignore())
                .ForMember(dest => dest.Eliminado, opt => opt.Ignore())
                .ForMember(dest => dest.HistoriaClinica, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore());
        }
    }
}