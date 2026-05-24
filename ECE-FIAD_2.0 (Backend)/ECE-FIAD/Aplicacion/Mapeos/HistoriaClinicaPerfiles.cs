using Aplicacion.DTOs.HistoriasClinicas;
using AutoMapper;
using Dominio.Entidades.HistoriasClinicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Mapeos
{
    public class HistoriaClinicaPerfiles : Profile
    {
        public HistoriaClinicaPerfiles()
        {
            CreateMap<CrearHistoriaClinicaDTO, HistoriaClinica>()
            .ForMember(dest => dest.Paciente, opt => opt.Ignore())
            .ForMember(dest => dest.Eliminado, opt => opt.Ignore())
            .ForMember(dest => dest.FechaDeCreacion, opt => opt.Ignore())
            .ForMember(dest => dest.FechaDeModificacion, opt => opt.Ignore())
            .ForMember(dest => dest.FechaDeEliminacion, opt => opt.Ignore());
            CreateMap<ActualizarHistoriaClinicaDTO, HistoriaClinica>()
            .ForMember(dest => dest.Paciente, opt => opt.Ignore())
            .ForMember(dest => dest.Activo, opt => opt.Ignore());
            CreateMap<HistoriaClinica, HistoriaClinicaDTO>().ReverseMap();

        }
    }
}
