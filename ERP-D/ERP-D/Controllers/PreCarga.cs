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

            return RedirectToAction("Index", "Home", new {mensaje = "Termine"});
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
