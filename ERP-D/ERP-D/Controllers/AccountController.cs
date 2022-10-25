//using AspNetCore;
using ERP_D.Models;
using ERP_D.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ERP_D.Controllers
{
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

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([Bind("DNI,Nombre,Apellido,Email,Password,ConfirmacionPassword")]RegistroEmpleado viewModel)
        {
            if (ModelState.IsValid)
            {
                Empleado empleadoACrear = new Empleado()
                {
                    DNI = viewModel.DNI,
                    Nombre = viewModel.Nombre,
                    Apellido = viewModel.Apellido,
                    Email = viewModel.Email,
                    UserName =  viewModel.Email
                    
                };

                var resultadoCreate = await _usermanager.CreateAsync(empleadoACrear, viewModel.Password);

                if (resultadoCreate.Succeeded)
                {
                    var resultadoRol = _usermanager.AddToRoleAsync(empleadoACrear, "Empleado");
                    await _signInManager.SignInAsync(empleadoACrear,isPersistent:false);
                    return RedirectToAction("Edit","Empleados", new {id = empleadoACrear.Id});
                }

                foreach(var error in resultadoCreate.Errors)
                {
                    ModelState.AddModelError(String.Empty,error.Description);
                }

            }
            return View(viewModel);
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Login viewModel)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.Recordarme, false);

                if (resultado.Succeeded)
                {
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

        public IActionResult Administrador()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }
    }
}
