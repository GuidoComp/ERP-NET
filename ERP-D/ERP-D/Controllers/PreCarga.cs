using ERP_D.Data;
using ERP_D.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ERP_D.Controllers
{
    public class PreCarga : Controller
    {
        private readonly UserManager<Persona> _userManager;
        private readonly RoleManager<Rol> _roleManager;
        private readonly ErpContext _erpContext;

        private List<String> roles = new List<String>() { "Admin", "RH", "Empleado", "Usuario" };

        public PreCarga(UserManager<Persona> userManager, RoleManager<Rol> roleManager, ErpContext erpContext)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._erpContext = erpContext;
        }

        public IActionResult Seed()
        {
            CrearRoles().Wait();
            CrearAdmin().Wait();
            
            return RedirectToAction("Index", "Home", new {mensaje = "Termine"});
        }

        private async Task CrearAdmin()
        {
            var admin = new Empleado();

            admin.Nombre = "admin";
            admin.Apellido = "admin";
            admin.Email = "admin@erp.com";
            admin.UserName = "admin@erp.com";

            var adminEncontrado = _erpContext.Personas.Any(p => p.Nombre == "Admin");

            if (!adminEncontrado)
            {
                var resultado = await _userManager.CreateAsync(admin, "Password1!");
                if (resultado.Succeeded)
                {
                    var resultadoRol = await _userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }

        private async Task CrearRoles()
        {
            foreach (var rolName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(rolName))
                {
                   await _roleManager.CreateAsync(new Rol(rolName));
                }
            }
        }
    }
}
