namespace ERP_D.Models
{
    public class ObraSocial
    {
        public enum TipoObraSocial
        {
            PRIVADA,
            ESTATAL,
        }

        public int NumeroAfiliado { get; set; }

        public TipoObraSocial Tipo { get; set; }

        public String Plan { get; set; }    
        
    }
}
