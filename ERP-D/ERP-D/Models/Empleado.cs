using ERP_D.Models;
using System.ComponentModel.DataAnnotations;

namespace ERP_D.Models
{
    public class Empleado : Persona
    { 
        public int Id { get;}
        public int Legajo { get; }

        public List<Telefono> Telefonos { get; set; }

        public Direccion Direccion { get; }

        public ObraSocial ObraSocial { get; }

        public bool EstaActivo { get; }

        public Posicion Posicion { get; }

        public Imagen Foto { get; set;}
    }
}