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