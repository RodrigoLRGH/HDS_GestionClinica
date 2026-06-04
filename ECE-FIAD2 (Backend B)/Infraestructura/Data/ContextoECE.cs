using Microsoft.EntityFrameworkCore;
using Dominio.Entidades.Citas;
using Dominio.Entidades.Doctores;
using Dominio.Entidades.Especialidades;
using Dominio.Entidades.Evoluciones;
using Dominio.Entidades.HistoriasClinicas;
using Dominio.Entidades.Pacientes;

namespace Infraestructura.Data
{
    public class ContextoECE : DbContext
    {
        public ContextoECE(DbContextOptions<ContextoECE> options) : base(options) { }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Doctor> Doctores { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<HistoriaClinica> HistoriasClinicas { get; set; }
        public DbSet<Evolucion> Evoluciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new Configuraciones.PacienteConfiguracion());
            modelBuilder.ApplyConfiguration(new Configuraciones.DoctorConfiguracion());
            modelBuilder.ApplyConfiguration(new Configuraciones.EspecialidadConfiguracion());
            modelBuilder.ApplyConfiguration(new Configuraciones.CitaConfiguracion());
            modelBuilder.ApplyConfiguration(new Configuraciones.HistoriaClinicaConfiguracion());
            modelBuilder.ApplyConfiguration(new Configuraciones.EvolucionConfiguracion());

            modelBuilder.Entity<Paciente>().HasQueryFilter(e => !e.Eliminado);
            modelBuilder.Entity<Doctor>().HasQueryFilter(e => !e.Eliminado);
            modelBuilder.Entity<Especialidad>().HasQueryFilter(e => !e.Eliminado);
            modelBuilder.Entity<Cita>().HasQueryFilter(e => !e.Eliminado);
            modelBuilder.Entity<HistoriaClinica>().HasQueryFilter(e => !e.Eliminado);
            modelBuilder.Entity<Evolucion>().HasQueryFilter(e => !e.Eliminado);
        }
    }
}