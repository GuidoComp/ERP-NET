namespace ERP_D.Models
{
    public class CentroDeCosto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal MontoMaximo { get; set; }
        public List<Gasto> Gastos { get; set; }
    }
}