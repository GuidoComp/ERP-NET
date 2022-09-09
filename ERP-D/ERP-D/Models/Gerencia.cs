using ERP_D.Models;
using System.ComponentModel.DataAnnotations;

namespace ERP_D.Models
{
    public class Gerencia
    {
        [Key]
        public int Id { get; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 2, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public Boolean EsGerenciaGeneral { get; init; }

        public Gerencia Direccion { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        public Posicion Responsable { get; set; }

        public List<Posicion> Posiciones { get; set; }

        public List<Gerencia> Gerencias { get; set; }

        public Empresa Empresa { get; init; }
    }
}
