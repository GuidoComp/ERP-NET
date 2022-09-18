using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;

namespace ERP_D.Models
{
    public abstract class Persona
    {
        
        public int Id { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public int DNI { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public String Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public String Apellido { get; set; }

        [EmailAddress(ErrorMessage = ErrorMsgs.MsgInvalido)]
        public String Email { get; set; }

        public string UserName { get; }

        public List<Telefono> Telefonos { get; set; }

        public Direccion Direccion { get; }
        public DateTime FechaAlta { get; set; }


    }
}
