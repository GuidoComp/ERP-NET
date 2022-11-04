using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_D.ViewModels.Imagenes
{
    public class SubirImagen
    {
        public String Nombre { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
