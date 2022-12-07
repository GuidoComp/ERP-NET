using ERP_D.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace ERP_D.Models
{
    public class Empleado : Persona
    {
        //Falta hacer que el valor de esta propiedad sea único. Cómo hacer que lo haga automáticamente? (Generated values, API fluente)
        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Range(1, 9999999, ErrorMessage = ErrorMsgs.MsgMonto)]
        public int Legajo { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Display(Name = "Obra social")]
        public ObraSocial ObraSocial { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Display(Name = "Empleado activo", AutoGenerateFilter = false)]
        public bool EmpleadoActivo { get; set; }

        //Posicion que está este empleado
        [ForeignKey("Posicion")]
        public int? PosicionId { get; set; }
        //Navegacional, para acceder a la posición de este empleado
        public Posicion Posicion { get; set; }

        public String Foto { get; set;}

        public List<Gasto> Gastos { get; set; }
    }
}