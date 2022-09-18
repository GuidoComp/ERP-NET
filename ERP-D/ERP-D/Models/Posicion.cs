using ERP_D.Models;

namespace ERP_D.Models
{
    public class Posicion
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public String Descripcion { get; set; }

        public double Sueldo { get; set; }

        public Empleado Empleado { get; set; }

        public Posicion Responsable { get; set; }

        public Gerencia Gerencia { get; set; }
    }
}
