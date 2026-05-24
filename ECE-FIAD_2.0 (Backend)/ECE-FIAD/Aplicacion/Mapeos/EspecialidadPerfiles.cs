using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Aplicacion.DTOs.Especialidades;
using Dominio.Entidades.Especialidades;

namespace Aplicacion.Mapeos
{
    public class EspeciliadaPerfiles : Profile
    {
        public EspeciliadaPerfiles()
        {
            CreateMap<CrearEspecialidadDTO, Especialidad>().ReverseMap();
            CreateMap<ActualizarEspecialidadDTO, Especialidad>().ReverseMap();
            CreateMap<Especialidad, EspecialidadDTO>().ReverseMap();
            CreateMap<Especialidad, EspecialidadDTO>()
    .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
    .ForMember(dest => dest.CantidadDoctores, opt => opt.MapFrom(src => src.Doctores.Count));
        }
    }
}
