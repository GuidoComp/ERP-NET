﻿using ERP_D.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace ERP_D.Models
{
    public class Empleado : Persona
    {

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Range(1, 9999999, ErrorMessage = ErrorMsgs.MsgMonto)]
        public int Legajo { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public ObraSocial ObraSocial { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public bool EmpleadoActivo { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [ForeignKey("Posicion")]
        public int PosicionId { get;  set; }

        public Posicion Posicion { get; set; }

        public String Foto { get; set;}

        public List<Gasto> Gastos { get; set; }
    }
}