using ERP_D.Data;
using ERP_D.Models;
using ERP_D.ViewModels.Organigrama;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Drawing.Text;

namespace ERP_D.Controllers
{
    public class OrganigramaController : Controller
    {
        private readonly ErpContext _context;
        private readonly UserManager<Persona> _userManager;

        public OrganigramaController(UserManager<Persona> userManager, ErpContext erpContext)
        {
            this._userManager = userManager;
            this._context = erpContext;
        }

        [Authorize(Roles = "Empleado, RH")]
        public async Task<IActionResult> Index(int? Id)
        {
            var org = new Organigrama();

            if (Id == null)
            {
                org = await BuscarNodoGerGeneral();
            }
            else{
                org = await BuscarNodoEspecifico((int)Id);
            }

            return View(org);
        }

        public async Task<IActionResult> TarjetaEmpleado(int? Id)
        {
            if (Id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.Include(e => e.Posicion).FirstOrDefaultAsync(m => m.Id == Id);

            if (empleado == null)
            {
                return NotFound();
            }

            var tarjetaEmpleado = new TarjetaEmpleado();

            tarjetaEmpleado.Nombre = empleado.Nombre;
            tarjetaEmpleado.Apellido = empleado.Apellido;
            tarjetaEmpleado.Email = empleado.Email;
            tarjetaEmpleado.Foto = empleado.Foto;
            tarjetaEmpleado.Direccion = empleado.Direccion;
            //tarjetaEmpleado.TipoTelefono = empleado.Telefonos;
            tarjetaEmpleado.Posicion = empleado.Posicion.Nombre;

            return View(tarjetaEmpleado);
        }

        private async Task<Organigrama> BuscarNodoEspecifico(int Id)
        {
            var org = new Organigrama();
            var gerencia = await _context.Gerencias.Include(g => g.Direccion).Include(g => g.Empresa).Include(g => g.SubGerencias).Include(g => g.Responsable).ThenInclude(r => r.Empleado).FirstOrDefaultAsync(g => g.Id == Id);
            
            org.Empresa = gerencia.Empresa.Nombre;
            org.GerenciaNombre = gerencia.Nombre;
            ViewBag.LogoEmp = gerencia.Empresa.Logo;

            if (gerencia.Responsable != null )
            {
                org.ResponsablePosicion = gerencia.Responsable.Nombre;
                if(gerencia.Responsable.Empleado != null)
                {
                    org.ResponsableNombre = gerencia.Responsable.Empleado.Nombre + " " + gerencia.Responsable.Empleado.Apellido;
                    org.ResponsableFoto = gerencia.Responsable.Empleado.Foto;
                }
            }

            if(gerencia.Direccion != null)
            {
                org.GerenciaResp = gerencia.Direccion.Nombre;
                org.GerenciaRespId = gerencia.Direccion.Id;
            }

            org.ListadoSubGerencias = gerencia.SubGerencias;

            var empleadosGerencia = _context.Empleados.Include(e => e.Posicion).OrderByDescending(e => e.Apellido).ThenBy(e => e.Nombre).Where(e => e.Posicion.GerenciaId == Id && e.EmpleadoActivo == true).ToList();

            org.ListadoEmpleados = empleadosGerencia;

            return org;
        }


        private async Task<Organigrama> BuscarNodoGerGeneral()
        {
            var org = new Organigrama();
            var id = Int32.Parse(_userManager.GetUserId(User));
            var empleadoActual = await _context.Empleados.Include(e => e.Posicion).ThenInclude(p => p.Gerencia).FirstOrDefaultAsync(e => e.Id == id);
            if (empleadoActual != null)
            {
                var idEmpresa = empleadoActual.Posicion.Gerencia.EmpresaId;
                var empresaActual = await _context.Empresas.Include(e => e.Gerencias).ThenInclude(g => g.Responsable).FirstOrDefaultAsync(e => e.Id == idEmpresa);
                ViewBag.LogoEmp = empresaActual.Logo;
                Gerencia gerenciaGeneral = null;
                var indexGeren = 0;
                while (gerenciaGeneral == null && indexGeren < empresaActual.Gerencias.Count)
                {
                    if (empresaActual.Gerencias[indexGeren].EsGerenciaGeneral)
                    {
                        gerenciaGeneral = empresaActual.Gerencias[indexGeren];
                    }
                    indexGeren++;
                }

                if (gerenciaGeneral != null)
                {
                    org.Empresa = empresaActual.Nombre;
                    org.GerenciaNombre = gerenciaGeneral.Nombre;
                    org.ListadoSubGerencias = gerenciaGeneral.SubGerencias;

                    var empleadosGerencia = _context.Posiciones.Include(p => p.Empleado).Where(p => p.GerenciaId == gerenciaGeneral.Id && p.Empleado.EmpleadoActivo == true).ToList();

                    if (gerenciaGeneral.ResponsableId != null)
                    {
                        org.ResponsablePosicion = gerenciaGeneral.Responsable.Nombre;
                        Empleado empleadoResp = null;
                        var index = 0;
                        while (empleadoResp == null && index < empleadosGerencia.Count)
                        {
                            if (empleadosGerencia[index].Id == gerenciaGeneral.ResponsableId)
                            {
                                empleadoResp = empleadosGerencia[index].Empleado;
                            }
                            index++;
                        }
                        if (empleadoResp != null)
                        {
                            org.ResponsableNombre = empleadoResp.Nombre + " " + empleadoResp.Apellido;
                            org.ResponsableFoto = empleadoResp.Foto;
                        }
                    }
                }
            }
            return org;
        }

    }
}


