using ERP_D.Helpers;
using ERP_D.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ERP_D.ViewModels
{
    public class CreacionEmpleado
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Display(Name = "Obra social")]
        public ObraSocial ObraSocial { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Display(Name = "Empleado activo", AutoGenerateFilter = false)]
        public bool EmpleadoActivo { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Display(Name = "RH", AutoGenerateFilter = false)]
        public bool RH { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [RegularExpression(@"^[\d]{1,2}\.?[\d]{3,3}\.?[\d]{3,3}$", ErrorMessage = ErrorMsgs.MsgFormatoInvalido)]
        public int DNI { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 1, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 1, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public string Apellido { get; set; }

        public string Foto { get; set; }

        public string Direccion { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public int PosicionId { get; set; }
    }
}
