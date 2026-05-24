using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Aplicacion.DTOs.Doctores;
using Dominio.Entidades.Doctores;

namespace Aplicacion.Mapeos
{
    public class DoctorPerfiles : Profile
    {
        public DoctorPerfiles()
        {
            // De DTO a Entidad (para crear)
            CreateMap<CrearDoctorDTO, Doctor>().ReverseMap();
            // Para actualización
            CreateMap<ActualizarDoctorDTO, Doctor>().ReverseMap();
            // Para Consultar
            CreateMap<Doctor, DoctorDTO>().ReverseMap();
            CreateMap<ActualizarDoctorDTO, Doctor>()
            .ForMember(dest => dest.Especialidad, opt => opt.Ignore())
            .ForMember(dest => dest.Citas, opt => opt.Ignore());
        }
    }
}
