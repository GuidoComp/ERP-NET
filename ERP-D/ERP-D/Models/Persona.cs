using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;

namespace ERP_D.Models
{
    public abstract class Persona
    {
        private const string requiredError = "El {0} es un campo obligatorio"; 


        [Required(ErrorMessage = requiredError)]
        public int DNI { get; set; }

        [Required(ErrorMessage = requiredError)]
        public String Nombre { get; set; }

        [Required(ErrorMessage = requiredError)]
        public String Apellido { get; set; }

        [EmailAddress(ErrorMessage = "Por favor, escriba un email valido")]
        public String Email { get; set; }

        public DateTime FechaAlta { get; set; }


    }
}
