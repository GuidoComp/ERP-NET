//using AspNetCore;
using ERP_D.Models;
using ERP_D.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ERP_D.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<Persona> _usermanager;
        private readonly SignInManager<Persona> _signInManager;
        private readonly RoleManager<Rol> _roleManager;

        public AccountController(UserManager<Persona> usermanager, SignInManager<Persona> signInManager, RoleManager<Rol> roleManager)
        {
            this._usermanager = usermanager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;

        }

        [AllowAnonymous]
        public IActionResult IniciarSesion(String returnUrl, string mensaje)
        {
            TempData["url"] = returnUrl;
            if (mensaje != null)
            {
                ViewBag.Precarg = "Precarga exitosa";
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Login viewModel)
        {

            String url = TempData["url"] as String;
            if (ModelState.IsValid)
            {
                var resultado = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.Recordarme, false);

                if (resultado.Succeeded)
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        return Redirect(url);
                    }
                   return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(String.Empty, "El inicio es invalido");
            }
            return View(viewModel);
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Administrador()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult AccesoDenegado(string redirectUrl)
        {
            return View();
        }
    }
}
