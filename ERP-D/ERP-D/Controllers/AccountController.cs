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
        private readonly SignInManager<Persona> _sigInManager;

        public AccountController(UserManager<Persona> usermanager, SignInManager<Persona> signInManager)
        {
            this._usermanager = usermanager;
            this._sigInManager = signInManager;

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
                    await _sigInManager.SignInAsync(empleadoACrear,isPersistent:false);
                    return RedirectToAction("Index","Home");
                }

                foreach(var error in resultadoCreate.Errors)
                {
                    ModelState.AddModelError(String.Empty,error.Description);
                }

            }
            return View(viewModel);
        }
    }
}
