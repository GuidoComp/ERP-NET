using ERP_D.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_D.Models
{
    public class Usuario : Persona
    {
        public string Username { get;}

        [PasswordPropertyText]
        public string Password { get; }

        public DateTime LastUpdatePassword { get; set; }

        public Empleado Empleado { get; set; }

        public Rol Rol { get; set; }
    }
}
