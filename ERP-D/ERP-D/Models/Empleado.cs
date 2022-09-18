using ERP_D.Models;
using System.ComponentModel.DataAnnotations;

namespace ERP_D.Models
{
    public class Empleado : Persona
    {
        // Este el ID del empleado
        [Key]
        public int Legajo { get; }

        public ObraSocial ObraSocial { get; }

        public bool EmpleadoActivo { get; }

        public Posicion Posicion { get; }

        public Imagen Foto { get; set;}
    }
}