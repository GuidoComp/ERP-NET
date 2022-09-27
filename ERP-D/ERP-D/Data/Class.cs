using ERP_D.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace ERP_D.Data
{
    public class ErpContext : DbContext
    {
        public ErpContext(DbContextOptions options) : base(options)
        {

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
