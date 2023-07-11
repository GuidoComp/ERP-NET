using ERP_D.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using ERP_D.Helpers;

namespace ERP_D.ViewModels.Posiciones
{
    public class EditPosicion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 1, ErrorMessage = ErrorMsgs.MsgStringLength)]
        [Remote(action: "NombreDisponible", controller: "Posiciones")]
        public String Nombre { get; set; }

        [StringLength(200, MinimumLength = 1, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Descripcion { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Range(1.00, 999999.00, ErrorMessage = ErrorMsgs.MsgMonto)]
        public decimal Sueldo { get; set; }

        [Display(Name = "Responsable")]
        [ForeignKey("Responsable")]
        public int? ResponsableId { get; set; }

        [Display(Name = "Gerencia")]
        [ForeignKey("Gerencia")]
        public int? GerenciaId { get; set; }

        [Display(Name = Alias.InfoGYE)]
        public string InfoGerenciaYEmpresa { get; set; }
    }
}
