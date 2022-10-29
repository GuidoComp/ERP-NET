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
        // EN nuestro proyecto no hay alta de usuario de esta manera. Lo hacen los usuarios RH
        //[AllowAnonymous]
        //public IActionResult Registrar()
        //{
        //    return View();
        //}
        //[AllowAnonymous]
        //[HttpPost]
        //public async Task<IActionResult> Registrar([Bind("DNI,Nombre,Apellido,Email,Password,ConfirmacionPassword")]RegistroEmpleado viewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Empleado empleadoACrear = new Empleado()
        //        {
        //            DNI = viewModel.DNI,
        //            Nombre = viewModel.Nombre,
        //            Apellido = viewModel.Apellido,
        //            Email = viewModel.Email,
        //            UserName =  viewModel.Email
                    
        //        };

        //        var resultadoCreate = await _usermanager.CreateAsync(empleadoACrear, viewModel.Password);

        //        if (resultadoCreate.Succeeded)
        //        {
        //            var resultadoRol = _usermanager.AddToRoleAsync(empleadoACrear, "Empleado");
        //            await _signInManager.SignInAsync(empleadoACrear,isPersistent:false);
        //            return RedirectToAction("Edit","Empleados", new {id = empleadoACrear.Id});
        //        }

        //        foreach(var error in resultadoCreate.Errors)
        //        {
        //            ModelState.AddModelError(String.Empty,error.Description);
        //        }

        //    }
        //    return View(viewModel);
        //}
        [AllowAnonymous]
        public IActionResult IniciarSesion(String returnUrl)
        {
            TempData["url"] = returnUrl;
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
