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
        public Boolean EsGerenciaGeneral { get; set; }

        [ForeignKey("Direccion")]
        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public int GerenciaId { get; set; } 

        public Gerencia Direccion { get; set; }

        [ForeignKey("Responsable")]
        public int PosicionResponsableId { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public Posicion Responsable { get; set; }

        [InverseProperty("Gerencia")] /// A CHEQUEAR
        public List<Posicion> Posiciones { get; set; }

        public List<Gerencia> SubGerencias { get; set; }

        [ForeignKey("Empresa")]
        public int EmpresaId { get; set; }

        public Empresa Empresa { get; set; }

        [ForeignKey("CentroDeCosto")]
        public int CentroDeCostoId { get; set; }

        public CentroDeCosto CentroDeCosto { get; set; }

        
    }
}
