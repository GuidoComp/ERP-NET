using ERP_D.Data;
using ERP_D.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ERP_D.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<Persona> _userManager;

        private readonly ErpContext _erpContext;

        private readonly SignInManager<Persona> _signInManager;

        public HomeController(UserManager<Persona> userManager, RoleManager<Rol> roleManager, ErpContext erpContext, ILogger<HomeController> logger, SignInManager<Persona> signInManager)
        {
            this._userManager = userManager;
            this._erpContext = erpContext;
            _logger = logger;
            this._signInManager = signInManager;
        }
        [AllowAnonymous]
        public IActionResult Landing(String mensaje)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
                
            }
            return View();
        }

        public async Task<IActionResult> Index(String mensaje)
        {
            ViewBag.mensaje = mensaje;
            if (_signInManager.IsSignedIn(User)){
                var userName = User.Identity.Name;
                Persona empleado = await _erpContext.Personas.FirstOrDefaultAsync(p => p.NormalizedUserName == userName.ToLower());
                ViewBag.fullName = empleado.Nombre + " " + empleado.Apellido;
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}