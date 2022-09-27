using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ERP_D.Models
{
    public class Gasto
    {
        public int Id { get; set; }

        [StringLength(300, MinimumLength = 10, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Descripcion { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Range(1, double.MaxValue, ErrorMessage = ErrorMsgs.MsgMonto)]
        public double Monto { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [ForeignKey("Empleado")]

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public int EmpleadoId { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public Empleado Empleado { get; set; }

        [ForeignKey("CentroDeCosto")]

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public int CentroDeCostoId { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public CentroDeCosto CentroDeCosto { get; set;} 


    }
}
