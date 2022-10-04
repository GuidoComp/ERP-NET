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

        [Display(Name = "Empleado")]
        [ForeignKey("Empleado")]
        public int? EmpleadoId { get; set; }
        public Empleado Empleado { get; set; }

        [Display(Name = "Responsable")]
        [ForeignKey("Responsable")]
        public int? ResponsableId { get; set; }
        public Posicion Responsable { get; set; }

        //A CHEQUEAR
        [Display(Name = "Posiciones que reportan")]
        [InverseProperty("Responsable")]
        public List<Posicion> PosicionesQueReportan { get; set; }


        [Display(Name = "Gerencia")]
        [ForeignKey("Gerencia")]
        public int? GerenciaId { get; set; }
        public Gerencia Gerencia { get; set; }
    }
}
