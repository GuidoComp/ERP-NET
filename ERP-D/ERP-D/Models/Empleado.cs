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
        public int PosicionId { get; set; }

        public Posicion Posicion { get; }

        [ForeignKey("Foto")]

        public int FotoId { get; set; }

        public Imagen Foto { get; set;}
    }
}