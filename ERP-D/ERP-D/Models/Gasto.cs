﻿namespace ERP_D.Models
{
    public class Gasto
    {
        public int Id { get;}
        public string Empleado { get; set; }
        public string Descripcion { get; set;}
        public double Monto { get; set;}    
        public DateTime Fecha { get; }
        public CentroDeCosto CentroDeCosto { get; set;} 


    }
}