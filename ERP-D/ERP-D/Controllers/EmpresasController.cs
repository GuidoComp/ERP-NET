using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP_D.Data;
using ERP_D.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using ERP_D.Helpers;
using ERP_D.ViewModels.Empresa;
using Microsoft.Data.SqlClient;

namespace ERP_D.Controllers
{
    [Authorize(Roles = "Admin, RH")]
    public class EmpresasController : Controller
    {
        private readonly ErpContext _context;

        public EmpresasController(ErpContext context)
        {
            _context = context;
        }

        // GET: Empresas
        public async Task<IActionResult> Index()
        {
            //(
            return View(await _context.Empresas.Include(e => e.Gerencias).ToListAsync());
        }

        // GET: Empresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Empresas == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .Include(e => e.Gerencias)
                .FirstOrDefaultAsync(m => m.Id == id);

            var gerencia = empresa.Gerencias.Find(g => g.EsGerenciaGeneral);
            if (gerencia != null)
            {
                ViewData["GerenciaGeneral"] = gerencia.Nombre;
            }

            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // GET: Empresas/Create
        [Authorize(Roles = "Admin, RH")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Rubro,Logo,Email,NombreCentro,MontoMaximo")] CreacionEmpresa empresaForm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var empresa = new Empresa();
                    empresa.Nombre = empresaForm.Nombre;
                    if (empresaForm.Logo != null)
                    {
                        var fotoPath = await Utils.CrearFoto(empresaForm.Logo, "", _context);
                        empresa.Logo = fotoPath;
                    }

                    empresa.Rubro = empresaForm.Rubro;
                    empresa.Email = empresaForm.Email;
                    _context.Add(empresa);
                    await _context.SaveChangesAsync();

                    var posicionCEO = new Posicion();
                    posicionCEO.Nombre = "CEO - " + empresaForm.Nombre;
                    posicionCEO.Sueldo = 500000;
                    _context.Add(posicionCEO);
                    await _context.SaveChangesAsync();


                    var gerencia = new Gerencia();
                    gerencia.Nombre = "Gerencia general - " + empresaForm.Nombre;
                    gerencia.EsGerenciaGeneral = true;
                    gerencia.ResponsableId = posicionCEO.Id;
                    gerencia.EmpresaId = empresa.Id;
                    _context.Add(gerencia);
                    await _context.SaveChangesAsync();

                    posicionCEO.GerenciaId = gerencia.Id;
                    _context.Update(posicionCEO);
                    await _context.SaveChangesAsync();

                    var nuevoCentro = new CentroDeCosto();

                    nuevoCentro.Nombre = empresaForm.Nombre;
                    nuevoCentro.MontoMaximo = empresaForm.MontoMaximo;
                    nuevoCentro.Gerencia = gerencia;
                    _context.CentrosDeCosto.Add(nuevoCentro);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbex)
                {
                    SqlException innerException = dbex.InnerException as SqlException;
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601)){
                        ModelState.AddModelError("Nombre", Errors.NombreDuplicadoError);
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, dbex.Message);
                    }
                }

            }
            return View(empresaForm);
        }

        // GET: Empresas/Edit/5
        [Authorize(Roles = "Admin, RH")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Empresas == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas.FindAsync(id);

            if (empresa == null)
            {
                return NotFound();
            }

            var empresaForm = new EdicionEmpresa();

            empresaForm.Id = empresa.Id;
            empresaForm.Nombre = empresa.Nombre;
            empresaForm.Rubro = empresa.Rubro;
            empresaForm.Email = empresa.Email;
            return View(empresaForm);
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, RH")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Rubro,Logo,Email")] EdicionEmpresa empresaForm)
        {
            if (id != empresaForm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var empresa = new Empresa();
                    empresa.Id = empresaForm.Id;
                    if (empresaForm.Logo != null)
                    {
                        var fotoPath = await Utils.CrearFoto(empresaForm.Logo, "", _context);
                        empresa.Logo = fotoPath;
                    }

                    empresa.Id = empresaForm.Id;
                    empresa.Nombre = empresaForm.Nombre;
                    empresa.Rubro = empresaForm.Rubro;
                    empresa.Email = empresaForm.Email;

                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbex)
                {
                    SqlException innerException = dbex.InnerException as SqlException;
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
                    {
                        ModelState.AddModelError("Nombre", Errors.NombreDuplicadoError);
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, dbex.Message);
                    }
                }
            }
            return View(empresaForm);
        }

        // GET: Empresas/Delete/5
        [Authorize(Roles = "Admin, RH")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Empresas == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, RH")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Empresas == null)
            {
                return Problem("Entity set 'ErpContext.Empresas'  is null.");
            }
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa != null)
            {
                _context.Empresas.Remove(empresa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult NombreDisponible(string nombre)
        {
            var nombreExistente = _context.Empresas.Any(g => g.Nombre == nombre);

            if (!nombreExistente)
            {
                return Json(true);
            }
            else
            {
                return Json(Errors.NombreDuplicadoError);
            }
        }
        private bool EmpresaExists(int id)
        {
          return _context.Empresas.Any(e => e.Id == id);
        }
    }
}
