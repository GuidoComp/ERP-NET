using System.ComponentModel.DataAnnotations;

namespace ERP_D.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public String Nombre { get; set; }
        public String Rubro { get; set; }
        public Imagen Logo { get; set; }
        public String Direccion { get; set; }

        public int TelefonoId { get; }
        public Telefono TelefonoContacto { get; set; }

        [EmailAddress(ErrorMessage = ErrorMsgs.MsgInvalido)]
        public String Email { get; set; }

    }
}