using ERP_D.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_D.Models
{
    public class Usuario : Persona
    {
        [PasswordPropertyText]
        public string Password { get; }

        public DateTime LastUpdatePassword { get; set; }

        public Empleado Empleado { get; set; }
    }
}
