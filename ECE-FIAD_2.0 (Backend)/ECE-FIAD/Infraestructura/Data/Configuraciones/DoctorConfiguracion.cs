using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dominio.Entidades.Doctores;
namespace Infraestructura.Data.Configuraciones
{
    public class DoctorConfiguracion : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctores");
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Nombres).IsRequired().HasMaxLength(100);
            builder.Property(d => d.Apellidos).IsRequired().HasMaxLength(100);
            builder.Property(d => d.Email).IsRequired().HasMaxLength(100);
            builder.HasIndex(d => d.Email).IsUnique();
            builder.HasOne(d => d.Especialidad)
            .WithMany(e => e.Doctores)
            .HasForeignKey(d => d.IdEspecialidad)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}