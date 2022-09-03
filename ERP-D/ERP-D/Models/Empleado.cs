namespace ERP_D.Models
{
    public class Empleado 
    {
        public string Nombre { get; private set; }
        public string Apellido { get; set; }
        public int DNI { get; set; }
        public List<Telefono> Telefonos { get; set; }
        public Direccion Direccion { get; set; }    
        public DateTime FechaAlta { get; set; }
        public string email { get; set; }
        public string ObraSocial { get; set; }
        public string Legajo { get; set; }
        public Boolean EmpleadoActivo { get; set; }
        public string Posicion { get; set; }
        public Boolean Foto { get; set; } //???????? FOTO??

    }
}