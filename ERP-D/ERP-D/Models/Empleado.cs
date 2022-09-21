using ERP_D.Models;
using System.ComponentModel.DataAnnotations;

namespace ERP_D.Models
{
    public class Empleado : Persona
    {
        // Este el ID del empleado
        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public int Legajo { get; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public String ObraSocial { get; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public bool EmpleadoActivo { get; }

        public int PosicionId { get; set; }

        public Posicion Posicion { get; }

        public Imagen Foto { get; set;}
    }
}