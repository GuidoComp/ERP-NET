using ERP_D.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ERP_D.ViewModels.Organigrama
{
    public class TarjetaEmpleado
    {
        public String Nombre { get; set; }
        public String Apellido { get; set; }
        public String Email { get; set; }
        public String Foto { get; set; }

        public String Direccion { get; set; }

        [Display(Name = "Tipo de telefono")]
        public Telefono.TipoTelefono TipoTelefono { get; set; }

        [Display(Name = "Telefono")]
        public String NumeroTelefono { get; set; }
        public String Posicion { get; set; }
    }
}
