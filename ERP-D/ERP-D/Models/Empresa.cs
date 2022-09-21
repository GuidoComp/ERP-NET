namespace ERP_D.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Rubro { get; set; }
        public Imagen Logo { get; set; }
        public String Direccion { get; set; }
        public Telefono TelefonoContacto { get; set; }
        public string Email { get; set; }

    }
}