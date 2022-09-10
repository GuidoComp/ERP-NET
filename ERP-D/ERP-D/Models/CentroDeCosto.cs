namespace ERP_D.Models
{
    public class CentroDeCosto
    {
        public int IdCentroDeCosto { get; set; }
        public string Nombre { get; set; }
        public double MontoMaximo { get; set; }
        public List<Gasto> Gastos { get; set; }
    }
}