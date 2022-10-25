using ERP_D.Data;
using ERP_D.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ERP_D.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<Persona> _userManager;

        private readonly ErpContext _erpContext;

        public HomeController(UserManager<Persona> userManager, RoleManager<Rol> roleManager, ErpContext erpContext, ILogger<HomeController> logger)
        {
            this._userManager = userManager;
            this._erpContext = erpContext;
            _logger = logger;
        }

        public async Task<IActionResult> Index(String mensaje)
        {
            //TODO: Migrar creacion de admin a landing page
            var admin = new Empleado();

            admin.Nombre = "admin";
            admin.Apellido = "admin";
            admin.Email = "admin@erp.com";
            admin.UserName = "admin@erp.com";

            var adminEncontrado =_erpContext.Personas.Any(p => p.Nombre == "Admin");

            if (!adminEncontrado)
            {
                var resultado = await _userManager.CreateAsync(admin, "Password1!");
                if (resultado.Succeeded)
                {
                    var resultadoRol = _userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            ViewBag.mensaje = mensaje;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}