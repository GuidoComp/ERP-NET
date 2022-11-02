using ERP_D.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_D.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public String Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public String Rubro { get; set; }
        public String Logo { get; set; }

        //[ForeignKey("TelefonoContacto")]
        //public int TelefonoId { get; set; }
        //public Telefono TelefonoContacto { get; set; }

        [EmailAddress(ErrorMessage = ErrorMsgs.MsgInvalido)]
        [Display(Name = Alias.email)]
        public String Email { get; set; }

        [InverseProperty("Empresa")] //VER SI SACARLO
        public List<Gerencia> Gerencias { get; set; }
        

    }
}