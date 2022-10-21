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

        [Display(Name = "Gerencia general")]
        [ForeignKey("GerenciaGeneral")]
        public int? GerenciaGeneralId { get; set; }
        public Gerencia GerenciaGeneral { get; set; }

        //Llevar a clase tema de cómo implementar este getDireccion, pero manteniendo el modelo anémico (prop autoimplementada) (necesario).

        public Gerencia getDireccion()
        {
            Gerencia direccion = null;
            foreach (Gerencia g in this.Gerencias)
            {
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
        [Display(Name = Alias.email)]
        public String Email { get; set; }

        [InverseProperty("Empresa")]
        public List<Gerencia> Gerencias { get; set; }
        

    }
}