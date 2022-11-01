using ERP_D.Helpers;
using ERP_D.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ERP_D.ViewModels.Empleados
{
    public class RegistroEmpleado
    {
        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [RegularExpression(@"^[\d]{1,2}\.?[\d]{3,3}\.?[\d]{3,3}$", ErrorMessage = ErrorMsgs.MsgFormatoInvalido)]
        public int DNI { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 1, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 1, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [EmailAddress(ErrorMessage = ErrorMsgs.MsgInvalido)]
        [Display(Name = Alias.email)]
        public string Email { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [DataType(DataType.Password)]
        [Display(Name = Alias.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [DataType(DataType.Password)]
        [Display(Name = Alias.ConfirmPassword)]
        [Compare("Password", ErrorMessage = ErrorMsgs.PassMissmatch)]
        public string ConfirmacionPassword { get; set; }
    }
}
