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

        public Gerencia Direccion;

        public Gerencia getDireccion()
        {
            Gerencia direccion = null;
            foreach (Gerencia g in Gerencias){
                if (g.EsGerenciaGeneral)
                {
                    direccion = g;
                }
            }
            return direccion;
        }
            
        //[ForeignKey("TelefonoContacto")]
        //public int TelefonoId { get; set; }
        //public Telefono TelefonoContacto { get; set; }

        [EmailAddress(ErrorMessage = ErrorMsgs.MsgInvalido)]
        public String Email { get; set; }

        public List<Gerencia> Gerencias { get; set; }
        

    }
}