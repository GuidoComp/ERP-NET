using ERP_D.Data;
using ERP_D.Helpers;
using ERP_D.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

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
            CrearEmpleados().Wait();
            CrearGastos().Wait();

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

                    var test = _erpContext.Posiciones.Add(new Posicion() { Nombre = "CEO", Sueldo = 1000000, GerenciaId = gerenciaDb.Id });
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
            var empleadoEncontrado = _erpContext.Empleados.Any();

            if (!empleadoEncontrado)
            {
                var posicionCEO = _erpContext.Posiciones.FirstOrDefault(p => p.Nombre == "CEO");
                var posicionGerenteCom = _erpContext.Posiciones.FirstOrDefault(p => p.Nombre == "Gerente comercial");
                var posicionGerenteOpe = _erpContext.Posiciones.FirstOrDefault(p => p.Nombre == "Gerente de operaciones");
                if (posicionCEO != null && posicionGerenteCom != null && posicionGerenteOpe != null)
                {
                    var empleadoCEO = new Empleado();

                    //empleadoCEO.Legajo = 1;
                    empleadoCEO.Nombre = "Marcos";
                    empleadoCEO.Apellido = "Lopez";
                    empleadoCEO.DNI = 23384556;
                    empleadoCEO.ObraSocial = ObraSocial.GALENO;
                    empleadoCEO.Direccion = "Callao 3532";
                    empleadoCEO.EmpleadoActivo = true;
                    empleadoCEO.Email = "marcos.lopez@ort.edu.ar";
                    empleadoCEO.UserName = "23384556";
                    empleadoCEO.NormalizedUserName = "23384556";
                    empleadoCEO.Foto = "test";
                    empleadoCEO.PosicionId = posicionCEO.Id;
                    await CrearUser(empleadoCEO, true);

                    var empleadoGerenteCom = new Empleado();

                    //empleadoGerenteCom.Legajo = 2;
                    empleadoGerenteCom.Nombre = "Maria";
                    empleadoGerenteCom.Apellido = "Perez";
                    empleadoGerenteCom.DNI = 20944855;
                    empleadoGerenteCom.ObraSocial = ObraSocial.MEDICUS;
                    empleadoGerenteCom.Direccion = "Rodriguez Peña 443";
                    empleadoGerenteCom.EmpleadoActivo = true;
                    empleadoGerenteCom.Email = "maria.perez@ort.edu.ar";
                    empleadoGerenteCom.UserName = "20944855";
                    empleadoGerenteCom.NormalizedUserName = "20944855";
                    empleadoGerenteCom.Foto = "test";
                    empleadoGerenteCom.PosicionId = posicionGerenteCom.Id;
                    await CrearUser(empleadoGerenteCom, false);

                    var empleadoGerenteOpe = new Empleado();

                    //empleadoGerenteOpe.Legajo = 1;
                    empleadoGerenteOpe.Nombre = "Laura";
                    empleadoGerenteOpe.Apellido = "Gonzalez";
                    empleadoGerenteOpe.DNI = 21445695;
                    empleadoGerenteOpe.ObraSocial = ObraSocial.OSDE;
                    empleadoGerenteOpe.Direccion = "Peñaflor 5667";
                    empleadoGerenteOpe.EmpleadoActivo = true;
                    empleadoGerenteOpe.Email = "laura.gonzalez@ort.edu.ar";
                    empleadoGerenteOpe.UserName = "21445695";
                    empleadoGerenteOpe.NormalizedUserName = "21445695";
                    empleadoGerenteOpe.Foto = "test";
                    empleadoGerenteOpe.PosicionId = posicionGerenteOpe.Id;
                    await CrearUser(empleadoGerenteOpe, false);
                }
            }
        }

        public async Task CrearUser(Persona empleado, bool RH)
        {
            var resultado = await _userManager.CreateAsync(empleado, empleado.DNI.ToString());
            if (resultado.Succeeded)
            {
                if (RH)
                {
                    await _userManager.AddToRoleAsync(empleado, "Empleado");
                    await _userManager.AddToRoleAsync(empleado, "RH");
                }
                else
                {
                    await _userManager.AddToRoleAsync(empleado, "Empleado");
                }
            }
        }

        public async Task CrearGastos()
        {
            var gastoEncontrado = _erpContext.Gastos.Any();

            if (!gastoEncontrado)
            {
                var centroDeCosto = _erpContext.CentrosDeCosto.FirstOrDefault();
                var empleados = _erpContext.Empleados.Select(e => e.Id).ToList();

                foreach (var empleado in empleados)
                {
                    crearGasto(empleado, centroDeCosto.Id);
                }
            }
        }

        private void crearGasto(int empleadoId, int centroDeCostoId)
        {
            for (int i = 0; i < 5; i++)
            {

                var gasto = new Gasto();
                Random rnd = new Random();

                gasto.Fecha = RandomDay();
                gasto.Monto = rnd.Next(1, 3000);
                gasto.CentroDeCostoId = centroDeCostoId;
                gasto.EmpleadoId = empleadoId;

                _erpContext.Gastos.Add(gasto);
                _erpContext.SaveChanges();
            }
        }

        private DateTime RandomDay()
        {
            Random gen = new Random();
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }
    }
}
