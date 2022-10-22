using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ERP_D.Models
{
    //No es necesaria esta clase, simplemente nos queda como guia
    public class Rol : IdentityRole<int>
    {
        public Rol() : base(){}

        public Rol(string name) : base(name){}

        //public int Id { get; set; }

        [Display(Name = "Rol")]
        public override string Name 
        {
            get{ return base.Name; }
            set { base.Name = value; } 
        }

        public override string NormalizedName
        {
            get => base.NormalizedName;
            set => base.NormalizedName = value;
        }
    }
}
