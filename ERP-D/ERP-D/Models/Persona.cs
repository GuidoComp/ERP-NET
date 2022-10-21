using ERP_D.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.ConstrainedExecution;

namespace ERP_D.Models
{
    public abstract class Persona
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [RegularExpression(@"^[\d]{1,2}\.?[\d]{3,3}\.?[\d]{3,3}$", ErrorMessage = ErrorMsgs.MsgFormatoInvalido)]
        public int DNI { get; set; }


        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 1 , ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 1, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Apellido { get; set; }

        [EmailAddress(ErrorMessage = ErrorMsgs.MsgInvalido)]
        [Display(Name = Alias.email)]
        public String? Email { get; set; }
        //TODO: GENERAR EMAIL A PARTIR DE NOMBRE

        public List<Telefono> Telefonos { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 4, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Direccion { get; set; }


        [Display(Name = "Fecha de alta")]
        [DataType(DataType.Date)]
        public DateTime? FechaAlta { get; set; }

        // Agregamos los atributos de usuario dentro de persona
        [Display(Name = "Usuario")]
        public String UserName { get; set; }

        [Display(Name = "Contraseña")]
        [PasswordPropertyText]
        public String Password { get; set; }

    }
}
