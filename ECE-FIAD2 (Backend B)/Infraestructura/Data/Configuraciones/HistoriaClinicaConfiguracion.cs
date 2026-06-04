using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dominio.Entidades.HistoriasClinicas;
namespace Infraestructura.Data.Configuraciones
{
    public class HistoriaClinicaConfiguracion : IEntityTypeConfiguration<HistoriaClinica>
    {
        public void Configure(EntityTypeBuilder<HistoriaClinica> builder)
        {
            builder.ToTable("HistoriasClinicas");
            builder.HasKey(h => h.Id);
            builder.Property(h => h.Alergias).HasMaxLength(500);
            builder.Property(h => h.AntecedentesFamiliares).HasMaxLength(500);
            builder.Property(h => h.AntecedentesPersonales).HasMaxLength(500);
        }
    }
}