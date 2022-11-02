using ERP_D.Data;
using ERP_D.Helpers;
using ERP_D.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            CrearEmpresa().Wait();
            CrearGerencias().Wait();
            CrearPosiciones().Wait();
            
            return RedirectToAction("Index", "Home", new {mensaje = "Termine"});
        }

        private async Task CrearAdmin()
        {
            var adminEncontrado = _erpContext.Personas.Any(p => p.Nombre == "Admin");

            if (!adminEncontrado)
            {

                    var admin = new Persona();

                    admin.Nombre = "Alejandro";
                    admin.Apellido = "Gonzalez";
                    admin.DNI = 25678900;
                    admin.PhoneNumber = "1145566990";
                    admin.FechaAlta = DateTime.Now;
                    admin.Email = "admin@ort.edu.ar";
                    admin.UserName = "admin@ort.edu.ar";

                    var resultado = await _userManager.CreateAsync(admin, Const.defaultPassword);
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

        private async Task CrearEmpresa()
        {
            var empresaEncontrada = _erpContext.Empresas.Any();

            if (!empresaEncontrada)
            {
                var empresa = new Empresa();

                empresa.Nombre = "Globant";
                empresa.Rubro = "Sistemas";
                empresa.Logo = String.Empty;
                empresa.Email = "globant@ort.com.ar";

                _erpContext.Empresas.Add(empresa);
                await _erpContext.SaveChangesAsync();
            }
        }

        private async Task CrearGerencias()
        {
            var centroDeCostoEncontrado = _erpContext.CentrosDeCosto.Any();

            if (!centroDeCostoEncontrado)
            {
                var centroDeCosto = new CentroDeCosto();

                centroDeCosto.Nombre = "Centro de costo 1";
                centroDeCosto.MontoMaximo = 250000;

                _erpContext.CentrosDeCosto.Add(centroDeCosto);
                var result = await _erpContext.SaveChangesAsync();

                if(result > 0)
                {
                    var gerenciaEncontrada = _erpContext.Gerencias.Any();

                    if (!gerenciaEncontrada)
                    {
                        var empresa = _erpContext.Empresas.FirstOrDefault();
                        var centroDeCostoDb = _erpContext.CentrosDeCosto.FirstOrDefault();
                        if (empresa != null)
                        {
                            var gerencia = new Gerencia();

                            gerencia.Nombre = "Gerencia General";
                            gerencia.EsGerenciaGeneral = true;
                            gerencia.EmpresaId = empresa.Id;
                            gerencia.CentroDeCostoId = centroDeCostoDb.Id;

                            _erpContext.Gerencias.Add(gerencia);
                            await _erpContext.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        private async Task CrearPosiciones()
        {
            var posicionEncontrada = _erpContext.Posiciones.Any();

            if (!posicionEncontrada)
            {
                var gerenciaDb = _erpContext.Gerencias.FirstOrDefault();

                if (gerenciaDb != null)
                {
                    #region Cargo El Jefe

                    _erpContext.Posiciones.Add(new Posicion() { Nombre = "CEO", Sueldo = 1000000, GerenciaId = gerenciaDb.Id });
                    var result = _erpContext.SaveChanges();

                    if (result > 0)
                    {
                        gerenciaDb.ResponsableId = _erpContext.Posiciones.FirstOrDefault().Id;
                        _erpContext.Gerencias.Update(gerenciaDb);
                        _erpContext.SaveChanges();
                    }

                    #endregion

                    #region Cargo subordinados
                    _erpContext.Posiciones.Add(new Posicion() { Nombre = "Gerente comercial", Sueldo = 500000, ResponsableId = _erpContext.Posiciones.FirstOrDefault().Id, GerenciaId = gerenciaDb.Id });
                    _erpContext.Posiciones.Add(new Posicion() { Nombre = "Gerente de operaciones", Sueldo = 500000, ResponsableId = _erpContext.Posiciones.FirstOrDefault().Id, GerenciaId = gerenciaDb.Id });
                    _erpContext.SaveChanges();
                    #endregion
                }
            }
        }

        private async Task CrearEmpleados()
        {
            //foreach (var rolName in roles)
            //{
            //    if (!await _roleManager.RoleExistsAsync(rolName))
            //    {
            //        await _roleManager.CreateAsync(new Rol(rolName));
            //    }
            //}
        }
    }
}
