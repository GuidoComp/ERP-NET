using System.ComponentModel.DataAnnotations;

namespace ERP_D.Models
{
    public class CentroDeCosto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 1, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Nombre { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = ErrorMsgs.MsgMonto)]
        [Display(Name = "Monto máximo")]
        public double MontoMaximo { get; set; }

        public Gerencia Gerencia { get; set; }
        public List<Gasto> Gastos { get; set; }
    }
}