using ERP_D.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ERP_D.ViewModels.Gerencia
{
    public class CreacionGerencia
    {
        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 1, ErrorMessage = ErrorMsgs.MsgStringLength)]
        [Remote (action:"NombreDisponible", controller:"Gerencias")]
        public String Nombre { get; set; }

        [Display(Name = "Es gerencia general")]
        public Boolean EsGerenciaGeneral { get; set; }

        //[Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Display(Name = "Direccion")]
        public int? GerenciaId { get; set; }


        [Display(Name = "Responsable")]
        public int? ResponsableId { get; set; }


        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Display(Name = "Empresa")]
        public int EmpresaId { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 1, ErrorMessage = ErrorMsgs.MsgStringLength)]
        [Display(Name = "Nombre centro de costo")]
        public String NombreCentro { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = ErrorMsgs.MsgMonto)]
        [Display(Name = "Monto máximo")]
        public double MontoMaximo { get; set; }
    }
}
