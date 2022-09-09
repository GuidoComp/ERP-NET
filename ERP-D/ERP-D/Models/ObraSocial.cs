namespace ERP_D.Models
{
    public class ObraSocial
    {
        public enum TipoObraSocial
        {
            OSDE,
            OSECAC,
            SWISS,
            GALENO
        }

        public int NumeroAfiliado { get; set; }

        public TipoObraSocial Nombre { get; set; }

        public String Plan { get; set; }    
        
    }
}
