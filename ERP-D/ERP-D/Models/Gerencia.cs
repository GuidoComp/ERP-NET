using ERP_D.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_D.Models
{
    public class Gerencia
    {
        [Key]
        public int Id { get; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 2, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public Boolean EsGerenciaGeneral { get; init; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public Gerencia Direccion { get; set; }

        [ForeignKey("Gerencia")]
        public int idDireccionGerencial { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public Posicion Responsable { get; set; }

        [ForeignKey("Posicion")]
        public int idPosicionResponsable { get; set; }

        public List<Posicion> Posiciones { get; set; }

        public List<Gerencia> Gerencias { get; set; }

        public Empresa Empresa { get; init; }

        [ForeignKey("Empresa")]
        public int idEmpresa { get; set; }

        public CentroDeCosto CentroDeCosto { get; set; }

        [ForeignKey("CentroDeCosto")]
        public int idCentroDeCosto { get; set; }
    }
}
