using System.ComponentModel.DataAnnotations;

namespace ERP_D.Models
{
    public class Telefono
    {
        [Key]
        public int id { get; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Range(10000000, 9999999999999, ErrorMessage = ErrorMsgs.MsgRango)]
        public int Numero { get; set; }

        public TipoTelefono Tipo { get; set; } 
    }
}
