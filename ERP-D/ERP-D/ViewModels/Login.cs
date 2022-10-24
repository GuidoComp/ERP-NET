using ERP_D.Helpers;
using ERP_D.Models;
using Microsoft.Build.Execution;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ERP_D.ViewModels
{
    public class Login
    {
        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [EmailAddress(ErrorMessage = ErrorMsgs.MsgInvalido)]
        [Display(Name = Alias.email)]
        public string Email { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [DataType(DataType.Password)]
        [Display(Name = Alias.Password)]
        public string Password { get; set; }

        public bool Recordarme { get; set; }
    }
}
