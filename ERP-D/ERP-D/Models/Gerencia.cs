using ERP_D.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_D.Models
{
    public class Gerencia
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 1, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Nombre { get; set; }


        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Display(Name = "Es gerencia general")]
        public Boolean EsGerenciaGeneral { get; set; }


        [ForeignKey("Direccion")]
        //[Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Display(Name = "Direccion")]
        public int? GerenciaId { get; set; } 
        public Gerencia Direccion { get; set; }


        [InverseProperty("Gerencia")]
        public List<Posicion> Posiciones { get; set; }

        //podrian crear una gerencia, sin asignarle aún el responsable
        //[Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Display(Name = "Responsable")]
        [ForeignKey("Responsable")]
        public int? ResponsableId { get; set; } 
        public Posicion Responsable { get; set; }

        public List<Gerencia> SubGerencias { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)] //Debería ser requerida la selección de una empresa
        [Display(Name = "Empresa")]
        [ForeignKey("Empresa")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

       
        [Display(Name = "Centro de costo")]
        [ForeignKey("CentroDeCosto")]
        public int? CentroDeCostoId { get; set; }
        public CentroDeCosto CentroDeCosto { get; set; }


    }
}
