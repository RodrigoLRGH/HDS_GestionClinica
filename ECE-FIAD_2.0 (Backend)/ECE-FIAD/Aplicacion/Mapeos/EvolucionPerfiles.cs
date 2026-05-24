using Aplicacion.DTOs.Evoluciones;
using AutoMapper;
using Dominio.Entidades.Evoluciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Mapeos
{
    public class EvolucionPerfiles : Profile
    {
        public EvolucionPerfiles()
        {
            CreateMap<CrearEvolucionDTO, Evolucion>().ReverseMap();
            CreateMap<ActualizarEvolucionDTO, Evolucion>().ReverseMap();
            CreateMap<Evolucion, EvolucionDTO>().ReverseMap();
            CreateMap<ActualizarEvolucionDTO, Evolucion>()
            .ForMember(dest => dest.HistoriaClinica, opt => opt.Ignore())
            .ForMember(dest => dest.Doctor, opt => opt.Ignore());
        }
    }
}
