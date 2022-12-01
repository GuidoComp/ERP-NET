using ERP_D.Helpers;
using ERP_D.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ERP_D.ViewModels.Empresa
{
    public class EdicionEmpresa
    {
        public int Id { get; set; }
        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public String Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public String Rubro { get; set; }
        public IFormFile Logo { get; set; }

        [EmailAddress(ErrorMessage = ErrorMsgs.MsgInvalido)]
        [Display(Name = Alias.email)]
        public String Email { get; set; }
    }
}
