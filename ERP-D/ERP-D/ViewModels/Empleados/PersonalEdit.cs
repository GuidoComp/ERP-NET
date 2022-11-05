using ERP_D.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ERP_D.ViewModels.Empleados
{
    public class PersonalEdit
    {
        public int Id { get; set; }

        [Display(Name = "Nombre foto")]
        public string NombreFoto { get; set; }

        public IFormFile Foto { get; set; }

        [Display(Name = "Tipo de telefono")]
        public Telefono.TipoTelefono TipoTelefono { get; set; }

        [RegularExpression(@"^(?:(?:00)?549?)?0?(?:11|[2368]\d)(?:(?=\d{0,2}15)\d{2})??\d{8}$", ErrorMessage = ErrorMsgs.MsgFormatoInvalido)]
        [Display(Name = "Telefono")]
        public String NumeroTelefono { get; set; }
    }
}
