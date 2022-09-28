using ERP_D.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace ERP_D.Data
{
    public partial class ErpContext : DbContext
    {
        public ErpContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Telefono>()
                .Property(t => t.Tipo)
                .HasConversion<string>();

            modelBuilder.Entity<Empleado>()
            .Property(e => e.FechaAlta)
            .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Empleado>()
                .Property(e => e.ObraSocial)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<CentroDeCosto> CentrosDeCosto { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<Gerencia> Gerencias { get; set; }
        public DbSet<Imagen> Imagenes { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Posicion> Posiciones { get; set; }
        public DbSet<Telefono> Telefonos { get; set; }

    }
}
