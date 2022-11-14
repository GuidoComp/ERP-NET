using ERP_D.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ERP_D.ViewModels
{
    public class CentroCostoLista
    {
        public int Id { get; set; }
        public String Nombre { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = ErrorMsgs.MsgMonto)]
        [Display(Name = "Monto máximo")]
        public double MontoMaximo { get; set; }

        public string Gerencia { get; set; }

        public double Total { get; set; }

    }
}
