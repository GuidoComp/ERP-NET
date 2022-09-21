namespace ERP_D.Models
{
    public class Gasto
    {
        public int Id { get;}
        public int EmpleadoId { get; set; }
        public string Descripcion { get; set;}
        public decimal Monto { get; set;}    
        public DateTime Fecha { get; }
        public CentroDeCosto CentroDeCosto { get; set;} 


    }
}
