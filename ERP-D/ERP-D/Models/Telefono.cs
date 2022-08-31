using System.ComponentModel.DataAnnotations;

namespace ERP_D.Models
{
    public class Telefono
    {
        [Range(100000,999999, ErrorMessage = "EL numero debe estar comprendido entre 100000 y 999999")]
        public int Numero { get; set; }
        public TipoTelefono tipoTelefono { get; set; }
    }
}
