using ERP_D.Data;
using ERP_D.Helpers;
using ERP_D.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
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

        public async Task<IActionResult> SeedAsync()
        {
            CrearRoles().Wait();
            CrearAdmin().Wait();
            //Task<Gerencia> TGerenciaGeneral = CrearEmpresa();
            //Gerencia gGeneralResult = await TGerenciaGeneral;
            //CrearGerencias(gGeneralResult).Wait();
            var gerenciaGeneral = await CrearEmpresa(); //La creación de la Empresa incluye una Gerencia General con una Posición correspondiente para el CEO y su centro de costo.
            CrearGerencias(gerenciaGeneral).Wait();
            CrearEmpleados().Wait();
            CrearGastos().Wait();

            return RedirectToAction("IniciarSesion", "Account", new {mensaje = "Termine"});
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
                //admin.Email = string.Format("admin{0}", Const.defaultEmail);
                //admin.UserName = admin.Email;

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

        private async Task<Gerencia> CrearEmpresa()
        {
            var empresaEncontrada = _erpContext.Empresas.Any();

            if (!empresaEncontrada)
            {
                var empresa = new Empresa();

                empresa.Nombre = "Globant";
                empresa.Rubro = "Sistemas";
                empresa.Logo = "/images/Globant.jpg.jpg";
                empresa.Email = "globant@ort.com.ar";

                _erpContext.Empresas.Add(empresa);
                await _erpContext.SaveChangesAsync();

                //var gerencia = new Gerencia();
                //var centroDeCostoDb = _erpContext.CentrosDeCosto.FirstOrDefault();

                //gerencia.Nombre = "Gerencia General";
                //gerencia.EsGerenciaGeneral = true;
                //gerencia.EmpresaId = empresa.Id;
                //gerencia.CentroDeCostoId = centroDeCostoDb.Id;

                //_erpContext.Gerencias.Add(gerencia);
                //await _erpContext.SaveChangesAsync();

                //var posicion = new Posicion() { Nombre = "CEO", Sueldo = 1000000, GerenciaId = gerencia.Id, InfoGerenciaYEmpresa = $"{empresa.Nombre} - {gerencia.Nombre}" };
                //_erpContext.Posiciones.Add(posicion);
                //await _erpContext.SaveChangesAsync();

                //gerencia.ResponsableId = posicion.Id;
                //_erpContext.Gerencias.Update(gerencia);
                //await _erpContext.SaveChangesAsync();

                //CrearGerencias(gResult).Wait();

                //Task<Gerencia> TGerenciaGeneral = CrearGerenciaGeneral(empresa);
                //Gerencia gGeneralResult = await TGerenciaGeneral;
                //return gGeneralResult;
                
                Gerencia gGeneralResult = await CrearGerenciaGeneral(empresa);
                return gGeneralResult;
            }
            return null;
        }

        private async Task<Gerencia> CrearGerenciaGeneral(Empresa empresa)
        {
            if (empresa != null)
            {
                var centroDeCostoEncontrado = _erpContext.CentrosDeCosto.Any();

                if (!centroDeCostoEncontrado)
                {
                    var centroDeCosto = new CentroDeCosto();

                    centroDeCosto.Nombre = "Centro de costo 1";
                    centroDeCosto.MontoMaximo = 25000000;

                    _erpContext.CentrosDeCosto.Add(centroDeCosto);
                    var result = await _erpContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        var gerenciaEncontrada = _erpContext.Gerencias.Any();

                        if (!gerenciaEncontrada)
                        {
                            var empresaEncontrada = _erpContext.Empresas.FirstOrDefault();
                            var centroDeCostoDb = _erpContext.CentrosDeCosto.FirstOrDefault();
                            if (empresaEncontrada != null)
                            {
                                var gerencia = new Gerencia();

                                gerencia.Nombre = "Gerencia General";
                                gerencia.EsGerenciaGeneral = true;
                                gerencia.EmpresaId = empresaEncontrada.Id;
                                gerencia.CentroDeCostoId = centroDeCostoDb.Id;

                                _erpContext.Gerencias.Add(gerencia);
                                await _erpContext.SaveChangesAsync();

                                var posicion = new Posicion() { Nombre = "CEO", Sueldo = 1000000, GerenciaId = gerencia.Id/*, InfoGerenciaYEmpresa = $"{empresa.Nombre} - {gerencia.Nombre}"*/ };
                                _erpContext.Posiciones.Add(posicion);
                                await _erpContext.SaveChangesAsync();

                                gerencia.ResponsableId = posicion.Id;
                                _erpContext.Gerencias.Update(gerencia);
                                await _erpContext.SaveChangesAsync();

                                return gerencia;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private async Task CrearGerencias(Gerencia gerenciaGeneral)
        {
            //var centroDeCostoEncontrado = _erpContext.CentrosDeCosto.Any();

            //if (!centroDeCostoEncontrado)
            //{
            //    var centroDeCosto = new CentroDeCosto();

            //    centroDeCosto.Nombre = "Centro de costo 1";
            //    centroDeCosto.MontoMaximo = 250000;

            //    _erpContext.CentrosDeCosto.Add(centroDeCosto);
            //    var result = await _erpContext.SaveChangesAsync();

            //    if(result > 0)
            //    {
            //        var gerenciaEncontrada = _erpContext.Gerencias.Any();

            //        if (!gerenciaEncontrada)
            //        {
            //            var empresa = _erpContext.Empresas.FirstOrDefault();
            //            var centroDeCostoDb = _erpContext.CentrosDeCosto.FirstOrDefault();
            //            if (empresa != null)
            //            {
            //                var gerencia = new Gerencia();

            //                gerencia.Nombre = "Gerencia General";
            //                gerencia.EsGerenciaGeneral = true;
            //                gerencia.EmpresaId = empresa.Id;
            //                gerencia.CentroDeCostoId = centroDeCostoDb.Id;

            //                _erpContext.Gerencias.Add(gerencia);
            //                await _erpContext.SaveChangesAsync();

            //                var posicion = new Posicion() { Nombre = "CEO", Sueldo = 1000000, GerenciaId = gerencia.Id/*, InfoGerenciaYEmpresa = $"{empresa.Nombre} - {gerencia.Nombre}"*/ };
            //                _erpContext.Posiciones.Add(posicion);
            //                await _erpContext.SaveChangesAsync();

            //                gerencia.ResponsableId = posicion.Id;
            //                _erpContext.Gerencias.Update(gerencia);
            //                await _erpContext.SaveChangesAsync();
            if (gerenciaGeneral != null)
            {
                await crearGerenciaOpe(gerenciaGeneral.Id);
                await crearGerenciaCome(gerenciaGeneral.Id);
                await crearGerenciaSis(gerenciaGeneral.Id);
                await crearGerenciaRH(gerenciaGeneral.Id);
            }
            //            }
            //        }
            //    }

            //}
        }

        private async Task crearGerenciaOpe(int idGerenciaGen)
        {
            var centroDeCosto = new CentroDeCosto();

            centroDeCosto.Nombre = "Centro operaciones";
            centroDeCosto.MontoMaximo = 5000000;

            _erpContext.CentrosDeCosto.Add(centroDeCosto);
            var result = await _erpContext.SaveChangesAsync();

            var empresa = _erpContext.Empresas.FirstOrDefault();
            var centroDeCostoDb = _erpContext.CentrosDeCosto.FirstOrDefault();
            if (empresa != null)
            {
                var gerencia = new Gerencia();

                gerencia.Nombre = "Gerencia operaciones";
                gerencia.EmpresaId = empresa.Id;
                gerencia.DireccionId = idGerenciaGen;
                gerencia.CentroDeCostoId = centroDeCosto.Id;

                _erpContext.Gerencias.Add(gerencia);
                await _erpContext.SaveChangesAsync();

                var posicion = new Posicion() { Nombre = "Gerente de operaciones", Sueldo = 500000, ResponsableId = _erpContext.Posiciones.FirstOrDefault().Id, GerenciaId = gerencia.Id/*, InfoGerenciaYEmpresa = $"{empresa.Nombre} - {gerencia.Nombre}"*/ };
                _erpContext.Posiciones.Add(posicion);
                await _erpContext.SaveChangesAsync();

                gerencia.ResponsableId = posicion.Id;
                _erpContext.Gerencias.Update(gerencia);
                await _erpContext.SaveChangesAsync();
            }
        }

        private async Task crearGerenciaCome(int idGerenciaGen)
        {
            var centroDeCosto = new CentroDeCosto();

            centroDeCosto.Nombre = "Centro comercial";
            centroDeCosto.MontoMaximo = 2000000;

            _erpContext.CentrosDeCosto.Add(centroDeCosto);
            var result = await _erpContext.SaveChangesAsync();

            var empresa = _erpContext.Empresas.FirstOrDefault();
            var centroDeCostoDb = _erpContext.CentrosDeCosto.FirstOrDefault();
            if (empresa != null)
            {
                var gerencia = new Gerencia();

                gerencia.Nombre = "Gerencia comercial";
                gerencia.EmpresaId = empresa.Id;
                gerencia.DireccionId = idGerenciaGen;
                gerencia.CentroDeCostoId = centroDeCosto.Id;

                _erpContext.Gerencias.Add(gerencia);
                await _erpContext.SaveChangesAsync();

                var posicion = new Posicion() { Nombre = "Gerente comercial", Sueldo = 500000, ResponsableId = _erpContext.Posiciones.FirstOrDefault().Id, GerenciaId = gerencia.Id/*, InfoGerenciaYEmpresa = $"{empresa.Nombre} - {gerencia.Nombre}"*/ };
                _erpContext.Posiciones.Add(posicion);
                await _erpContext.SaveChangesAsync();

                gerencia.ResponsableId = posicion.Id;
                _erpContext.Gerencias.Update(gerencia);
                await _erpContext.SaveChangesAsync();
            }
        }

        private async Task crearGerenciaSis(int idGerenciaGen)
        {
            var centroDeCosto = new CentroDeCosto();

            centroDeCosto.Nombre = "Centro sistemas";
            centroDeCosto.MontoMaximo = 3000000;

            _erpContext.CentrosDeCosto.Add(centroDeCosto);
            var result = await _erpContext.SaveChangesAsync();

            var empresa = _erpContext.Empresas.FirstOrDefault();
            var centroDeCostoDb = _erpContext.CentrosDeCosto.FirstOrDefault();
            if (empresa != null)
            {
                var gerencia = new Gerencia();

                gerencia.Nombre = "Gerencia sistemas";
                gerencia.EmpresaId = empresa.Id;
                gerencia.DireccionId = idGerenciaGen;
                gerencia.CentroDeCostoId = centroDeCosto.Id;

                _erpContext.Gerencias.Add(gerencia);
                await _erpContext.SaveChangesAsync();

                var posicion = new Posicion() { Nombre = "Gerente sistemas", Sueldo = 500000, ResponsableId = _erpContext.Posiciones.FirstOrDefault().Id, GerenciaId = gerencia.Id/*, InfoGerenciaYEmpresa = $"{empresa.Nombre} - {gerencia.Nombre}"*/ };
                _erpContext.Posiciones.Add(posicion);
                await _erpContext.SaveChangesAsync();

                gerencia.ResponsableId = posicion.Id;
                _erpContext.Gerencias.Update(gerencia);
                await _erpContext.SaveChangesAsync();
            }
        }

        private async Task crearGerenciaRH(int idGerenciaGen)
        {
            var centroDeCosto = new CentroDeCosto();

            centroDeCosto.Nombre = "Centro RH";
            centroDeCosto.MontoMaximo = 200000;

            _erpContext.CentrosDeCosto.Add(centroDeCosto);
            var result = await _erpContext.SaveChangesAsync();

            var empresa = _erpContext.Empresas.FirstOrDefault();
            var centroDeCostoDb = _erpContext.CentrosDeCosto.FirstOrDefault();
            if (empresa != null)
            {
                var gerencia = new Gerencia();

                gerencia.Nombre = "Gerencia RH";
                gerencia.EmpresaId = empresa.Id;
                gerencia.DireccionId = idGerenciaGen;
                gerencia.CentroDeCostoId = centroDeCosto.Id;

                _erpContext.Gerencias.Add(gerencia);
                await _erpContext.SaveChangesAsync();
                var posicion = new Posicion() { Nombre = "Gerente RH", Sueldo = 500000, ResponsableId = _erpContext.Posiciones.FirstOrDefault().Id, GerenciaId = gerencia.Id/*, InfoGerenciaYEmpresa = $"{empresa.Nombre} - {gerencia.Nombre}"*/ };
                _erpContext.Posiciones.Add(posicion);
                await _erpContext.SaveChangesAsync();

                gerencia.ResponsableId = posicion.Id;
                _erpContext.Gerencias.Update(gerencia);
                await _erpContext.SaveChangesAsync();

                await crearGerenciaRecruiting(gerencia.Id);
            }
        }

        private async Task crearGerenciaRecruiting(int idGerenciaGen)
        {
            var centroDeCosto = new CentroDeCosto();

            centroDeCosto.Nombre = "Centro Recruiting";
            centroDeCosto.MontoMaximo = 200000;

            _erpContext.CentrosDeCosto.Add(centroDeCosto);
            var result = await _erpContext.SaveChangesAsync();

            var empresa = _erpContext.Empresas.FirstOrDefault();
            var centroDeCostoDb = _erpContext.CentrosDeCosto.FirstOrDefault();
            if (empresa != null)
            {
                var gerencia = new Gerencia();

                gerencia.Nombre = "Gerencia recruiting";
                gerencia.EmpresaId = empresa.Id;
                gerencia.DireccionId = idGerenciaGen;
                gerencia.CentroDeCostoId = centroDeCosto.Id;

                _erpContext.Gerencias.Add(gerencia);
                await _erpContext.SaveChangesAsync();

                var posicion = new Posicion() { Nombre = "Gerente recruiting", Sueldo = 500000, ResponsableId = _erpContext.Posiciones.FirstOrDefault(p => p.Nombre == "Gerente RH").Id, GerenciaId = gerencia.Id/*, InfoGerenciaYEmpresa = $"{empresa.Nombre} - {gerencia.Nombre}"*/ };
                _erpContext.Posiciones.Add(posicion);
                await _erpContext.SaveChangesAsync();

                var posicion1 = new Posicion() { Nombre = "Recruiter 1", Sueldo = 1000, ResponsableId = _erpContext.Posiciones.FirstOrDefault(p => p.Nombre == "Gerente recruiting").Id, GerenciaId = gerencia.Id/*, InfoGerenciaYEmpresa = $"{empresa.Nombre} - {gerencia.Nombre}"*/ };
                _erpContext.Posiciones.Add(posicion1);
                await _erpContext.SaveChangesAsync();

                var posicion2 = new Posicion() { Nombre = "Recruiter 2", Sueldo = 2000, ResponsableId = _erpContext.Posiciones.FirstOrDefault(p => p.Nombre == "Gerente recruiting").Id, GerenciaId = gerencia.Id/*, InfoGerenciaYEmpresa = $"{empresa.Nombre} - {gerencia.Nombre}"*/ };
                _erpContext.Posiciones.Add(posicion2);
                await _erpContext.SaveChangesAsync();

                gerencia.ResponsableId = posicion.Id;
                _erpContext.Gerencias.Update(gerencia);
                await _erpContext.SaveChangesAsync();
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
                var posicionGerenteSis = _erpContext.Posiciones.FirstOrDefault(p => p.Nombre == "Gerente sistemas");
                var posicionGerenteRH = _erpContext.Posiciones.FirstOrDefault(p => p.Nombre == "Gerente RH");
                var posicionGerenteRecr = _erpContext.Posiciones.FirstOrDefault(p => p.Nombre == "Gerente recruiting");
                var posicionRecruiter1 = _erpContext.Posiciones.FirstOrDefault(p => p.Nombre == "Recruiter 1");
                var posicionRecruiter2 = _erpContext.Posiciones.FirstOrDefault(p => p.Nombre == "Recruiter 2");
                if (posicionCEO != null && posicionGerenteCom != null && posicionGerenteOpe != null)
                {
                    var empleadoCEO = new Empleado();

                    //empleadoCEO.Legajo = 1;
                    empleadoCEO.Nombre = "Marcos";
                    empleadoCEO.Apellido = "Lopez";
                    empleadoCEO.DNI = 23384556;
                    empleadoCEO.ObraSocial = ObraSocial.GALENO;
                    empleadoCEO.Direccion = "Callao 3532";
                    empleadoCEO.Foto = "/images/Marcos.jfif.jpg";
                    empleadoCEO.EmpleadoActivo = true; 
                    //empleadoCEO.Email = "marcos.lopez@ort.edu.ar";
                    //empleadoCEO.UserName = "23384556";
                    //empleadoCEO.NormalizedUserName = "23384556";
                    empleadoCEO.Email = "empleadorrhh1@ort.edu.ar";
                    empleadoCEO.UserName = empleadoCEO.Email;
                    empleadoCEO.NormalizedUserName = empleadoCEO.UserName.ToUpper();
                    //empleadoCEO.NormalizedUserName = "EMPLEADORRHH1@ORT.EDU.AR";
                    //empleadoCEO.Email = String.Format("{0}.{1}{2}", empleadoCEO.Nombre.ToLower(), empleadoCEO.Apellido.ToLower(), Const.defaultEmail);
                    //empleadoCEO.UserName = empleadoCEO.Email;
                    //empleadoCEO.NormalizedUserName = empleadoCEO.UserName.ToUpper();
                    empleadoCEO.PosicionId = posicionCEO.Id;
                    await CrearUser(empleadoCEO, true);

                    var telefonoCEO = new Telefono();
                    telefonoCEO.Tipo = Telefono.TipoTelefono.CELULAR;
                    telefonoCEO.Numero = "1168223399";
                    telefonoCEO.PersonaId = empleadoCEO.Id;
                    _erpContext.Telefonos.Add(telefonoCEO);
                    _erpContext.SaveChanges();

                    var empleadoGerenteCom = new Empleado();

                    //empleadoGerenteCom.Legajo = 2;
                    empleadoGerenteCom.Nombre = "Carmen";
                    empleadoGerenteCom.Apellido = "Perez";
                    empleadoGerenteCom.DNI = 20944855;
                    empleadoGerenteCom.ObraSocial = ObraSocial.MEDICUS;
                    empleadoGerenteCom.Direccion = "Rodriguez Peña 443";
                    empleadoGerenteCom.Foto = "/images/Carmen.jfif.jpg";
                    empleadoGerenteCom.EmpleadoActivo = true;
                    empleadoGerenteCom.Email = "empleado1@ort.edu.ar";
                    empleadoGerenteCom.UserName = empleadoGerenteCom.Email;
                    empleadoGerenteCom.NormalizedUserName = empleadoGerenteCom.UserName.ToUpper();
                    //empleadoGerenteCom.NormalizedUserName = "EMPLEADO1@ORT.EDU.AR";
                    //empleadoGerenteCom.Email = "maria.perez@ort.edu.ar";
                    //empleadoGerenteCom.UserName = "20944855";
                    //empleadoGerenteCom.NormalizedUserName = "20944855";
                    //empleadoGerenteCom.Email = String.Format("{0}.{1}{2}", empleadoGerenteCom.Nombre.ToLower(), empleadoGerenteCom.Apellido.ToLower(), Const.defaultEmail);
                    //empleadoGerenteCom.UserName = empleadoGerenteCom.Email;
                    //empleadoGerenteCom.NormalizedUserName = empleadoGerenteCom.UserName.ToUpper();
                    empleadoGerenteCom.PosicionId = posicionGerenteCom.Id;
                    await CrearUser(empleadoGerenteCom, false);

                    var telefonoGerenenteCom = new Telefono();
                    telefonoGerenenteCom.Tipo = Telefono.TipoTelefono.CELULAR;
                    telefonoGerenenteCom.Numero = "1134332208";
                    telefonoGerenenteCom.PersonaId = empleadoGerenteCom.Id;
                    _erpContext.Telefonos.Add(telefonoGerenenteCom);
                    _erpContext.SaveChanges();

                    var empleadoGerenteOpe = new Empleado();

                    //empleadoGerenteOpe.Legajo = 1;
                    empleadoGerenteOpe.Nombre = "Laura";
                    empleadoGerenteOpe.Apellido = "Gonzalez";
                    empleadoGerenteOpe.DNI = 21445695;
                    empleadoGerenteOpe.ObraSocial = ObraSocial.OSDE;
                    empleadoGerenteOpe.Direccion = "Peñaflor 5667";
                    empleadoGerenteOpe.EmpleadoActivo = true;
                    empleadoGerenteOpe.Foto = "/images/Laura.jfif.jpg";
                    //empleadoGerenteOpe.Email = String.Format("{0}.{1}{2}", empleadoGerenteOpe.Nombre.ToLower(), empleadoGerenteOpe.Apellido.ToLower(), Const.defaultEmail);
                    //empleadoGerenteOpe.UserName = empleadoGerenteOpe.Email;
                    //empleadoGerenteOpe.NormalizedUserName = empleadoGerenteOpe.UserName.ToUpper();
                    empleadoGerenteOpe.Email = "empleado2@ort.edu.ar";
                    empleadoGerenteOpe.UserName = empleadoGerenteOpe.Email;
                    empleadoGerenteOpe.NormalizedUserName = empleadoGerenteOpe.UserName.ToUpper();
                    //empleadoGerenteOpe.Email = "laura.gonzalez@ort.edu.ar";
                    //empleadoGerenteOpe.UserName = "21445695";
                    //empleadoGerenteOpe.NormalizedUserName = "21445695";
                    empleadoGerenteOpe.PosicionId = posicionGerenteOpe.Id;
                    await CrearUser(empleadoGerenteOpe, false);

                    var telefonoGerenenteOpe= new Telefono();
                    telefonoGerenenteOpe.Tipo = Telefono.TipoTelefono.CELULAR;
                    telefonoGerenenteOpe.Numero = "1123887312";
                    telefonoGerenenteOpe.PersonaId = empleadoGerenteOpe.Id;
                    _erpContext.Telefonos.Add(telefonoGerenenteOpe);
                    _erpContext.SaveChanges();

                    var empleadoGerenteSis = new Empleado();
                    empleadoGerenteSis.Nombre = "Rodrigo";
                    empleadoGerenteSis.Apellido = "Varae";
                    empleadoGerenteSis.DNI = 23456788;
                    empleadoGerenteSis.ObraSocial = ObraSocial.OSDE;
                    empleadoGerenteSis.Direccion = "Yatai 22";
                    empleadoGerenteSis.EmpleadoActivo = true;
                    empleadoGerenteSis.Foto = "/images/Rodrigo.jfif.jpg";
                    //empleadoGerenteSis.Email = String.Format("{0}.{1}{2}", empleadoGerenteSis.Nombre.ToLower(), empleadoGerenteSis.Apellido.ToLower(), Const.defaultEmail);
                    //empleadoGerenteSis.UserName = empleadoGerenteSis.Email;
                    //empleadoGerenteSis.NormalizedUserName = empleadoGerenteSis.UserName.ToUpper();
                    empleadoGerenteSis.Email = "empleado3@ort.edu.ar";
                    empleadoGerenteSis.UserName = empleadoGerenteSis.Email;
                    empleadoGerenteSis.NormalizedUserName = empleadoGerenteSis.UserName.ToUpper();
                    //empleadoGerenteSis.Email = "rodrigo.varae@ort.edu.ar";
                    //empleadoGerenteSis.UserName = "23456788";
                    //empleadoGerenteSis.NormalizedUserName = "23456788";
                    empleadoGerenteSis.PosicionId = posicionGerenteSis.Id;
                    await CrearUser(empleadoGerenteSis, false);

                    var telefonoGerenenteSis = new Telefono();
                    telefonoGerenenteSis.Tipo = Telefono.TipoTelefono.CELULAR;
                    telefonoGerenenteSis.Numero = "1193887212";
                    telefonoGerenenteSis.PersonaId = empleadoGerenteSis.Id;
                    _erpContext.Telefonos.Add(telefonoGerenenteSis);
                    _erpContext.SaveChanges();

                    var empleadoGerenteRH = new Empleado();
                    empleadoGerenteRH.Nombre = "Romina";
                    empleadoGerenteRH.Apellido = "Fernandez";
                    empleadoGerenteRH.DNI = 32475888;
                    empleadoGerenteRH.ObraSocial = ObraSocial.OMINT;
                    empleadoGerenteRH.Direccion = "Cabildo 2322";
                    empleadoGerenteRH.EmpleadoActivo = true;
                    empleadoGerenteRH.Foto = "/images/Romina.jfif.jpg";
                    //empleadoGerenteRH.Email = String.Format("{0}.{1}{2}", empleadoGerenteRH.Nombre.ToLower(), empleadoGerenteRH.Apellido.ToLower(), Const.defaultEmail);
                    //empleadoGerenteRH.UserName = empleadoGerenteRH.Email;
                    //empleadoGerenteRH.NormalizedUserName = empleadoGerenteRH.UserName.ToUpper();
                    empleadoGerenteRH.Email = "empleadorrhh2@ort.edu.ar";
                    empleadoGerenteRH.UserName = empleadoGerenteRH.Email;
                    empleadoGerenteRH.NormalizedUserName = empleadoGerenteRH.UserName.ToUpper();
                    //empleadoGerenteRH.Email = "romina.fernandez@ort.edu.ar";
                    //empleadoGerenteRH.UserName = "32475888";
                    //empleadoGerenteRH.NormalizedUserName = "32475888";
                    empleadoGerenteRH.PosicionId = posicionGerenteRH.Id;
                    await CrearUser(empleadoGerenteRH, true);

                    var telefonoGerenenteRH = new Telefono();
                    telefonoGerenenteRH.Tipo = Telefono.TipoTelefono.CELULAR;
                    telefonoGerenenteRH.Numero = "1165223464";
                    telefonoGerenenteRH.PersonaId = empleadoGerenteRH.Id;
                    _erpContext.Telefonos.Add(telefonoGerenenteRH);
                    _erpContext.SaveChanges();

                    var empleadoGerenteRecr = new Empleado();
                    empleadoGerenteRecr.Nombre = "Juana";
                    empleadoGerenteRecr.Apellido = "Villa";
                    empleadoGerenteRecr.DNI = 33322888;
                    empleadoGerenteRecr.ObraSocial = ObraSocial.OMINT;
                    empleadoGerenteRecr.Direccion = "Av Lopez 3432";
                    empleadoGerenteRecr.EmpleadoActivo = true;
                    empleadoGerenteRecr.Foto = "/images/Juana.jfif";
                    //empleadoGerenteRecr.Email = String.Format("{0}.{1}{2}", empleadoGerenteRecr.Nombre.ToLower(), empleadoGerenteRecr.Apellido.ToLower(), Const.defaultEmail);
                    //empleadoGerenteRecr.UserName = empleadoGerenteRecr.Email;
                    //empleadoGerenteRecr.NormalizedUserName = empleadoGerenteRecr.UserName.ToUpper();
                    empleadoGerenteRecr.Email = "empleadorrhh3@ort.edu.ar";
                    empleadoGerenteRecr.UserName = empleadoGerenteRecr.Email;
                    empleadoGerenteRecr.NormalizedUserName = empleadoGerenteRecr.Email.ToUpper();
                    //empleadoGerenteRecr.Email = "juana.villa@ort.edu.ar";
                    //empleadoGerenteRecr.UserName = "33322888";
                    //empleadoGerenteRecr.NormalizedUserName = "33322888";
                    empleadoGerenteRecr.PosicionId = posicionGerenteRecr.Id;
                    await CrearUser(empleadoGerenteRecr, true);


                    var telefonoGerenenteRecr = new Telefono();
                    telefonoGerenenteRecr.Tipo = Telefono.TipoTelefono.CELULAR;
                    telefonoGerenenteRecr.Numero = "1145667788";
                    telefonoGerenenteRecr.PersonaId = empleadoGerenteRecr.Id;
                    _erpContext.Telefonos.Add(telefonoGerenenteRecr);
                    _erpContext.SaveChanges();

                    var empleadoRecruiter1 = new Empleado();
                    empleadoRecruiter1.Nombre = "Jorge";
                    empleadoRecruiter1.Apellido = "Gonzalez";
                    empleadoRecruiter1.DNI = 21344022;
                    empleadoRecruiter1.ObraSocial = ObraSocial.GALENO;
                    empleadoRecruiter1.Direccion = "Correa 3432";
                    empleadoRecruiter1.EmpleadoActivo = true;
                    empleadoRecruiter1.Foto = "/images/jorge.jfif.jpg";
                    //empleadoRecruiter1.Email = String.Format("{0}.{1}{2}", empleadoRecruiter1.Nombre.ToLower(), empleadoRecruiter1.Apellido.ToLower(), Const.defaultEmail);
                    //empleadoRecruiter1.UserName = empleadoRecruiter1.Email;
                    //empleadoRecruiter1.NormalizedUserName = empleadoRecruiter1.UserName.ToUpper();
                    empleadoRecruiter1.Email = "empleadorrhh4@ort.edu.ar";
                    empleadoRecruiter1.UserName = empleadoRecruiter1.Email;
                    empleadoRecruiter1.NormalizedUserName = empleadoRecruiter1.UserName.ToUpper();
                    //empleadoRecruiter1.Email = "jorge.gonzalez@ort.edu.ar";
                    //empleadoRecruiter1.UserName = "21344022";
                    //empleadoRecruiter1.NormalizedUserName = "21344022";
                    empleadoRecruiter1.PosicionId = posicionRecruiter1.Id;
                    await CrearUser(empleadoRecruiter1, true);

                    var telefonoRecruiter1 = new Telefono();
                    telefonoRecruiter1.Tipo = Telefono.TipoTelefono.CELULAR;
                    telefonoRecruiter1.Numero = "1122993322";
                    telefonoRecruiter1.PersonaId = empleadoRecruiter1.Id;
                    _erpContext.Telefonos.Add(telefonoRecruiter1);
                    _erpContext.SaveChanges();

                    var empleadoRecruiter2 = new Empleado();
                    empleadoRecruiter2.Nombre = "Maria Rosa";
                    empleadoRecruiter2.Apellido = "Faller";
                    empleadoRecruiter2.DNI = 19233812;
                    empleadoRecruiter2.ObraSocial = ObraSocial.SANCOR_SALUD;
                    empleadoRecruiter2.Direccion = "Larrea 200";
                    empleadoRecruiter2.EmpleadoActivo = true;
                    empleadoRecruiter2.Foto = "/images/Maria Rosa.jpg";
                    //empleadoRecruiter2.Email = String.Format("{0}.{1}{2}", empleadoRecruiter2.Nombre.ToLower(), empleadoRecruiter2.Apellido.ToLower(), Const.defaultEmail);
                    //empleadoRecruiter2.UserName = empleadoRecruiter2.Email;
                    //empleadoRecruiter2.NormalizedUserName = empleadoRecruiter2.UserName.ToUpper();
                    //empleadoRecruiter2.Email = "maria.rosaa@ort.edu.ar";
                    //empleadoRecruiter2.UserName = "19233812";
                    //empleadoRecruiter2.NormalizedUserName = "19233812";
                    empleadoRecruiter2.Email = "empleadorrhh5@ort.edu.ar";
                    empleadoRecruiter2.UserName = empleadoRecruiter2.Email;
                    empleadoRecruiter2.NormalizedUserName = empleadoRecruiter2.UserName.ToUpper();
                    empleadoRecruiter2.PosicionId = posicionRecruiter2.Id;
                    await CrearUser(empleadoRecruiter2, true);

                    var telefonoRecruiter2 = new Telefono();
                    telefonoRecruiter2.Tipo = Telefono.TipoTelefono.CELULAR;
                    telefonoRecruiter2.Numero = "1134931233";
                    telefonoRecruiter2.PersonaId = empleadoRecruiter2.Id;
                    _erpContext.Telefonos.Add(telefonoRecruiter2);
                    _erpContext.SaveChanges();
                }
            }
        }

        public async Task CrearUser(Persona empleado, bool RH)
        {
            var resultado = await _userManager.CreateAsync(empleado, Const.defaultPassword);
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
                var empleados = _erpContext.Empleados.Include(e => e.Posicion).ThenInclude(p => p.Gerencia).ThenInclude(g => g.CentroDeCosto).ToList();

                foreach (var empleado in empleados)
                {
                    crearGasto(empleado.Id, (int)empleado.Posicion.Gerencia.CentroDeCostoId, (int)empleado.Posicion.Gerencia.CentroDeCosto.MontoMaximo);
                }
            }
        }

        private void crearGasto(int empleadoId, int centroDeCostoId, int montoMaximo)
        {
            for (int i = 0; i < 5; i++)
            {
                var gasto = new Gasto();
                Random rnd = new Random();

                gasto.Descripcion = "Caja chica";
                gasto.Fecha = RandomDay();
                gasto.Monto = rnd.Next(1, montoMaximo);
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
