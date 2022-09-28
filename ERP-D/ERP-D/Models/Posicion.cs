using ERP_D.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_D.Models
{
    public class Posicion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 1, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Nombre { get; set; }

        [StringLength(200, MinimumLength = 1, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Descripcion { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Range(1.00, 999999.00, ErrorMessage = ErrorMsgs.MsgMonto)]
        public decimal Sueldo { get; set; }

        [ForeignKey("Empleado")]
        public int? EmpleadoId { get; set; }
        public Empleado Empleado { get; set; }

        [ForeignKey("Responsable")]
        public int? ResponsableId { get; set; }
        public Posicion Responsable { get; set; }

        [ForeignKey("Gerencia")]
        public int? GerenciaId { get; set; }
        public Gerencia Gerencia { get; set; }
    }
}
