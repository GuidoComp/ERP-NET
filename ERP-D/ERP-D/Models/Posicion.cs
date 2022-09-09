using ERP_D.Models;

namespace ERP_D.Models
{
    public class Posicion
    {
        public int IdPosicion { get; set; }

        public String Descripcion { get; set; }

        public double Sueldo { get; set; }

        public List<Empleado> Empleados { get; set; }

        public Empleado Responsable { get; set; }

        public Gerencia GerenciaResponsable { get; set; }
    }
}
