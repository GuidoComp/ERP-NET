using ERP_D.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_D.Models
{
    public class Empleado : Persona
    {
        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public int Legajo { get; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public String ObraSocial { get; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public bool EmpleadoActivo { get; }

        [ForeignKey("Posicion")]
        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public int PosicionId { get; set; }

        public Posicion Posicion { get; }

        public String Foto { get; set;}
    }
}