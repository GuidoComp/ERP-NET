using System.ComponentModel.DataAnnotations;

namespace ERP_D.Models
{
    public class CentroDeCosto
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(10, MinimumLength = 2, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Nombre { get; set; }

        [Range(1.00, 9999999.00, ErrorMessage = ErrorMsgs.MsgMonto)]
        public decimal MontoMaximo { get; set; }

        public List<Gasto> Gastos { get; set; }
    }
}