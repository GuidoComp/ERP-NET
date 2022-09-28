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

        

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [ForeignKey("Empleado")]
        public int EmpleadoId { get; set; }
        public Empleado Empleado { get; set; }

        

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [ForeignKey("CentroDeCosto")]
        public int CentroDeCostoId { get; set; }
        public CentroDeCosto CentroDeCosto { get; set;} 


    }
}
