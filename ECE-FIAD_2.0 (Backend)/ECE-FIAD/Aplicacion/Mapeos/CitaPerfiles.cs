using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.Citas;
using AutoMapper;
using Dominio.Entidades.Citas;

namespace Aplicacion.Mapeos
{
    public class CitaPerfiles : Profile
    {
        public CitaPerfiles()
        {
            CreateMap<CrearCitaDTO, Cita>().ReverseMap();
            // Para actualización
            CreateMap<ActualizarCitaDTO, Cita>()
                .ForMember(dest => dest.Doctor, opt => opt.Ignore())
                .ForMember(dest => dest.Paciente, opt => opt.Ignore())
                .ForMember(dest => dest.Eliminado, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeEliminacion, opt => opt.Ignore());
            // Para Consultar
            CreateMap<Cita, CitaDTO>().ReverseMap();
        }
    }
}
