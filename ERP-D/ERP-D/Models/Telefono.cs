using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ERP_D.Models
{
    public class Telefono
    {

        public enum TipoTelefono
        {
            CELULAR,
            FIJO,
            OTRO
        }

        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
       // [RegularExpression(@"^(?:(?:00)?549?)?0?(?:11|[2368]\d)(?:(?=\d{0,2}15)\d{2})??\d{8}$", ErrorMessage = ErrorMsgs.MsgFormatoInvalido)]
        // REVISAR REGEX PARA QUE ACEPTE NUMEROS NO CELULARES (5411)
        public String Numero { get; set; } 


        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public TipoTelefono Tipo { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [ForeignKey("Persona")]
        public int PersonaId { get; set; }
        public Persona Persona { get; set; }
    }
}
