using ERP_D.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_D.Models
{
    public class Posicion
    {       
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [StringLength(70, MinimumLength = 1, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Nombre { get; set; }

        [StringLength(200, MinimumLength = 1, ErrorMessage = ErrorMsgs.MsgStringLength)]
        public String Descripcion { get; set; }

        [Required(ErrorMessage = ErrorMsgs.MsgReq)]
        [Range(1.00, 999999.00, ErrorMessage = ErrorMsgs.MsgMonto)]
        public decimal Sueldo { get; set; }

        [Display(Name = "Responsable")]
        [ForeignKey("Responsable")]

                
        public int? ResponsableId { get; set; } //Hay que ponerlo en esta instancia como nulleable, porque sino, al no estár fisponible cuando consulten las posiciones en el index por ejemplo, al hacer un include, por fedecto, excluirá toda posición que no tiene el responsable.
        
        //Navegacional para ver la posición de mi jefe.
        public Posicion Responsable { get; set; }

        //Navegacional para ver que empleado está en esta posición
        public Empleado Empleado { get; set; }


        //Navegacional para ver las posiciones Dependientes a esta posición
        public List<Posicion> Subordinadas { get; set; }

        [Display(Name = "Gerencia")]
        [ForeignKey("Gerencia")]
        public int? GerenciaId { get; set; }
        public Gerencia Gerencia { get; set; }
    }
}
