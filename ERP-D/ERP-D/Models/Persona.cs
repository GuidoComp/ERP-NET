using System.ComponentModel.DataAnnotations;

namespace ERP_D.Models
{
    public class Persona
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(50)]
        [MinLength(2)]
        public string Nombre { get; set; }
        public string Apellido { get; set; }    
        public Direccion Direccion { get; set; }
        public List<Telefono> Telefonos { get; set; }


    }

    
    {
    }
}
