using System.ComponentModel.DataAnnotations;

namespace ERP_D.Models
{
    public class Direccion
    {
        [Key]
        public int id { get; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(30, MinimumLength = 2, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public string Calle { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Range(1, 999999, ErrorMessage = ErrorMsgs.MsgRango)]
        public int Numero { get; set; }

        public int Piso { get; set; }

        public string Ciudad { get; set; }
    }
}