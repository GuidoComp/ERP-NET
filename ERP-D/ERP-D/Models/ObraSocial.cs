namespace ERP_D.Models
{
    public class ObraSocial
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
        public enum TipoObraSocial
        {
            OBRASOCIAL,
            PREPAGA,
        }

        public int NumeroAfiliado { get; set; }

        public TipoObraSocial Tipo { get; set; }

        public String Plan { get; set; }    
        
    }
}
