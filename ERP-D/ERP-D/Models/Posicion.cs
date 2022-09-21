using ERP_D.Models;
using System.ComponentModel.DataAnnotations;

namespace ERP_D.Models
{
    public class Posicion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 2, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Nombre { get; set; }

        [StringLength(70, MinimumLength = 2, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Descripcion { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Range(1.00, 999999.00, ErrorMessage = ErrorMsgs.MsgMonto)]
        public decimal Sueldo { get; set; }

        public int EmpleadoId { get; set; }
        public Empleado Empleado { get; set; }

        public int ResponsableId { get; set; }
        public Posicion Responsable { get; set; }

        public int GerenciaId { get; set; }
        public Gerencia Gerencia { get; set; }
    }
}
