using System.ComponentModel.DataAnnotations;

namespace ERP_D.Models
{
    public class Telefono
    {
        [Key]
        public int Id { get; }

        //[Required(ErrorMessage = ErrorMsgs.MsgReq)]
        //[Range(10000000, 9999999999999, ErrorMessage = ErrorMsgs.MsgRango)]
        //public int Numero { get; set; }

        [RegularExpression(@"([0-9]{2}\[0-9]{4}\[0-9]{4}) | ([0-9]{3}\[0-9]{3}\[0-9]{4}) | ([0-9]{4}\[0-9]{2}\[0-9]{4})", ErrorMessage = ErrorMsgs.MsgFormatoInvalido)]
        [DataType(DataType.PhoneNumber)]
        public string Numero { get; set; }

        public TipoTelefono Tipo { get; set; }
    }
}
