﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.ConstrainedExecution;

namespace ERP_D.Models
{
    public abstract class Persona
    {

        public Persona()
        {
            this.FechaAlta = DateTime.Now;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [RegularExpression(@"^[\d]{1,3}\.?[\d]{3,3}\.?[\d]{3,3}$", ErrorMessage = ErrorMsgs.MsgFormatoInvalido)]
        public int DNI { get; set; }


        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 1 , ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 1, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Apellido { get; set; }

        [EmailAddress(ErrorMessage = ErrorMsgs.MsgInvalido)]
        public String Email { get; set; }  //???? VER El email de la persona no es requerido, porque lo generarán uds. en base a alguna definición de nomenclatura? Podría ser interesante. Comentar a este respecto.

        public List<Telefono> Telefonos { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 4, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Direccion { get; set; }

        //  [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [DataType(DataType.Date)]
        public DateTime? FechaAlta { get; private set; }

        // Agregamos los atributos de usuario dentro de persona
        public String UserName { get; set; }

        [PasswordPropertyText]
        public String Password { get; set; }

    }
}
