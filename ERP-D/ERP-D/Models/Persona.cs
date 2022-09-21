using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;

namespace ERP_D.Models
{
    public abstract class Persona
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [RegularExpression(@"^[\d]{1,3}\.?[\d]{3,3}\.?[\d]{3,3}$", ErrorMessage = ErrorMsgs.MsgFormatoInvalido)]
        public int DNI { get; set; }


        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(15, MinimumLength = 3, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(15, MinimumLength = 3, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Apellido { get; set; }

        [EmailAddress(ErrorMessage = ErrorMsgs.MsgInvalido)]
        public String Email { get; set; }

        public List<Telefono> Telefonos { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public String Direccion { get; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [DataType(DataType.Date)]
        public DateTime FechaAlta { get; set; }

        // Agregamos los atributos de usuario dentro de persona
        public String UserName { get; }

        [PasswordPropertyText]
        public String Password { get; }

    }
}
