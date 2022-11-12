using ERP_D.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ERP_D.ViewModels.Organigrama
{  
    public class Organigrama
    {

        public string Empresa { get; set; }

        [Display(Name = "Nombre gerencia")]
        public string GerenciaNombre { get; set; }

        [Display(Name = "Responsable")]
        public string ResponsableNombre { get; set; }

        [Display(Name = "Posicion responsable")]
        public string ResponsablePosicion { get; set; }

        [Display(Name = "Foto responsable")]
        public string ResponsableFoto { get; set; }

        [Display(Name = "Gerencia padre")]
        public int GerenciaRespId { get; set; }


        [Display(Name = "Gerencia padre")]
        public string GerenciaResp { get; set; }

        [Display(Name = "Sub gerencias")]
        public List<ERP_D.Models.Gerencia> ListadoSubGerencias { get; set; }

        public List<ERP_D.Models.Empleado> ListadoEmpleados { get; set; }
    }
}
