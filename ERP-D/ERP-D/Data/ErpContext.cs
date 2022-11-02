using ERP_D.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Xml;

namespace ERP_D.Data
{
    public partial class ErpContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        public ErpContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Posicion>().Property(pos => pos.Sueldo).HasPrecision(38, 18);

            #region Configuracion incremental para legajo
            modelBuilder.HasSequence<int>("Legajo")
                .StartsAt(100)
                .IncrementsBy(1);

            modelBuilder.Entity<Empleado>().Property(e => e.Legajo)
                .HasDefaultValueSql("NEXT VALUE FOR Legajo");
            #endregion

            modelBuilder.Entity<Telefono>()
                .Property(t => t.Tipo)
                .HasConversion<string>();


            modelBuilder.Entity<Empleado>()
            .Property(e => e.FechaAlta)
            .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Empleado>()
                .HasIndex(e => e.DNI)
                .IsUnique();

            modelBuilder.Entity<Empleado>()
                .Property(e => e.ObraSocial)
                .HasConversion<string>();

            //Establecemos los nombres de Identity Stores

            modelBuilder.Entity<IdentityUser<int>>().ToTable("Personas");

            modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");

            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("PersonasRoles");

            
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

        public DbSet<Rol> Roles { get; set; }

    }
}
