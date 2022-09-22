using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_D.Models
{
    public class Telefono
    {
        public int Id { get; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [RegularExpression(@"^(?:(?:00)?549?)?0?(?:11|[2368]\d)(?:(?=\d{0,2}15)\d{2})??\d{8}$", ErrorMessage = ErrorMsgs.MsgFormatoInvalido)]
        [DataType(DataType.PhoneNumber)]
        public String Numero { get; set; }

        [ForeignKey("Tipo")] //Definimos una relación entre el Enum Tipo, y el telefono?
        public int TipoId { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public TipoTelefono Tipo { get; set; }
    }
}
