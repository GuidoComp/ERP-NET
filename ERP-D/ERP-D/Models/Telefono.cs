using System.ComponentModel.DataAnnotations;

namespace ERP_D.Models
{
    public class Telefono
    {
        [Key]
        public int Id { get; }

        [RegularExpression(@"^(?:(?:00)?549?)?0?(?:11|[2368]\d)(?:(?=\d{0,2}15)\d{2})??\d{8}$", ErrorMessage = ErrorMsgs.MsgFormatoInvalido)]
        [DataType(DataType.PhoneNumber)]
        public string Numero { get; set; }

        public TipoTelefono Tipo { get; set; }
    }
}
