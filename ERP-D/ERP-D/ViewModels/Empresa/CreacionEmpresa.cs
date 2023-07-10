using ERP_D.Helpers;
using ERP_D.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ERP_D.ViewModels.Empresa
{
    public class CreacionEmpresa
    {
        public int Id { get; set; }
        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Remote(action: "NombreDisponible", controller: "Empresas")]
        public String Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public String Rubro { get; set; }
        public IFormFile Logo { get; set; }

        //[ForeignKey("TelefonoContacto")]
        //public int TelefonoId { get; set; }
        //public Telefono TelefonoContacto { get; set; }

        [EmailAddress(ErrorMessage = ErrorMsgs.MsgInvalido)]
        [Display(Name = Alias.email)]
        public String Email { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 1, ErrorMessage = ErrorMsgs.MsgStringLength)]
        [Display(Name = "Nombre centro de costo")]
        public String NombreCentro { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = ErrorMsgs.MsgMonto)]
        [Display(Name = "Monto máximo")]
        public double MontoMaximo { get; set; }

    }
}
