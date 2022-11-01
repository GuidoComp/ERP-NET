using ERP_D.Models;
using System.ComponentModel.DataAnnotations;

namespace ERP_D.ViewModels.Gastos
{
    public class CrearGastoEmp
    {
        [StringLength(300, MinimumLength = 10, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Descripcion { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Range(1, double.MaxValue, ErrorMessage = ErrorMsgs.MsgMonto)]
        public double Monto { get; set; }
    }
}
