using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;

namespace ERP_D.Models
{
    public abstract class Persona
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public int DNI { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public String Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public String Apellido { get; set; }

        [EmailAddress(ErrorMessage = ErrorMsgs.MsgInvalido)]
        public String Email { get; set; }


        public List<Telefono> Telefonos { get; set; }

        public String Direccion { get; }

        public DateTime FechaAlta { get; set; }

        // Agregamos los atributos de usuario dentro de persona
        public string UserName { get; }

        [PasswordPropertyText]
        public string Password { get; }

    }
}
