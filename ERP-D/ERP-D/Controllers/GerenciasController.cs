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
using ERP_D.ViewModels.Gerencia;
using ERP_D.Helpers;

namespace ERP_D.Controllers
{
    [Authorize(Roles = "Admin, RH")]
    public class GerenciasController : Controller
    {
        private readonly ErpContext _context;

        public GerenciasController(ErpContext context)
        {
            _context = context;
        }

        // GET: Gerencias
        public async Task<IActionResult> Index()
        {
            var erpContext = _context.Gerencias.Include(g => g.CentroDeCosto).Include(g => g.Direccion).Include(g => g.Empresa).Include(g => g.Responsable);
            return View(await erpContext.ToListAsync());
        }

        // GET: Gerencias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Gerencias == null)
            {
                return NotFound();
            }

            var gerencia = await _context.Gerencias
                .Include(g => g.CentroDeCosto)
                .Include(g => g.Direccion)
                .Include(g => g.Empresa)
                .Include(g => g.Responsable)
                .Include(g => g.SubGerencias)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gerencia == null)
            {
                return NotFound();
            }

            return View(gerencia);
        }

        // GET: Gerencias/Create
        public IActionResult Create()
        {
            ViewBag.AnyGerenciaGeneral = _context.Gerencias.Any(g => g.EsGerenciaGeneral == true);

            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "Id", "Nombre");
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Nombre");
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "Id", "Nombre");
            return View();
        }

        // POST: Gerencias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,EsGerenciaGeneral,GerenciaId,ResponsableId,EmpresaId,NombreCentro,MontoMaximo")] CreacionGerencia gerenciaForm)
        {
            if (ModelState.IsValid)
            {
                var nuevoCentro = new CentroDeCosto();

                nuevoCentro.Nombre = gerenciaForm.Nombre;
                nuevoCentro.MontoMaximo = gerenciaForm.MontoMaximo;
                _context.CentrosDeCosto.Add(nuevoCentro);
                await _context.SaveChangesAsync();

                var nuevaGerencia = new Gerencia();

                nuevaGerencia.Nombre = gerenciaForm.Nombre;
                if (gerenciaForm.EsGerenciaGeneral)
                {
                    nuevaGerencia.EsGerenciaGeneral = gerenciaForm.EsGerenciaGeneral;
                }
                nuevaGerencia.DireccionId = gerenciaForm.GerenciaId;
                nuevaGerencia.ResponsableId = gerenciaForm.ResponsableId;
                nuevaGerencia.EmpresaId = gerenciaForm.EmpresaId;
                nuevaGerencia.CentroDeCostoId = nuevoCentro.Id;
                
                _context.Gerencias.Add(nuevaGerencia);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "Id", "Nombre", gerenciaForm.GerenciaId);
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Nombre", gerenciaForm.EmpresaId);
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "Id", "Nombre", gerenciaForm.ResponsableId);
            return View(gerenciaForm);
        }

        // GET: Gerencias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Gerencias == null)
            {
                return NotFound();
            }

            var gerencia = await _context.Gerencias.FindAsync(id);
            if (gerencia == null)
            {
                return NotFound();
            }
            ViewBag.AnyGerenciaGeneral = _context.Gerencias.Any(g => g.EsGerenciaGeneral == true);
            ViewData["CentroDeCostoId"] = new SelectList(_context.CentrosDeCosto, "Id", "Nombre", gerencia.CentroDeCostoId);
            ViewData["DireccionId"] = new SelectList(_context.Gerencias, "Id", "Nombre", gerencia.DireccionId);
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Nombre", gerencia.EmpresaId);
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "Id", "Nombre", gerencia.ResponsableId);
            //ViewData["GerenciaId"] = new SelectList(_context.Gerencias.Include(g => g.Empresa), "Id", "ObtenerEmpresaNombre");
            return View(gerencia);
        }

        // POST: Gerencias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,EsGerenciaGeneral,DireccionId,ResponsableId,EmpresaId,CentroDeCostoId")] EditGerencia gerenciaForm)
        {
            if (id != gerenciaForm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var gerencia = new Gerencia();
                    gerencia.Id = gerenciaForm.Id;
                    gerencia.Nombre = gerenciaForm.Nombre;
                    gerencia.EsGerenciaGeneral = gerenciaForm.EsGerenciaGeneral;
                    gerencia.DireccionId = gerenciaForm.DireccionId;
                    gerencia.ResponsableId = gerenciaForm.ResponsableId;
                    gerencia.EmpresaId = gerenciaForm.EmpresaId;
                    gerencia.CentroDeCostoId = gerenciaForm.CentroDeCostoId;

                    _context.Update(gerencia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GerenciaExists(gerenciaForm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CentroDeCostoId"] = new SelectList(_context.CentrosDeCosto, "Id", "Nombre", gerenciaForm.CentroDeCostoId);
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "Id", "Nombre", gerenciaForm.DireccionId);
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Nombre", gerenciaForm.EmpresaId);
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "Id", "Nombre", gerenciaForm.ResponsableId);
            return View(gerenciaForm);
        }

        // GET: Gerencias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Gerencias == null)
            {
                return NotFound();
            }

            var gerencia = await _context.Gerencias
                .Include(g => g.CentroDeCosto)
                .Include(g => g.Direccion)
                .Include(g => g.Empresa)
                .Include(g => g.Responsable)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gerencia == null)
            {
                return NotFound();
            }

            return View(gerencia);
        }

        // POST: Gerencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Gerencias == null)
            {
                return Problem("Entity set 'ErpContext.Gerencias'  is null.");
            }
            var gerencia = await _context.Gerencias.FindAsync(id);
            if (gerencia != null)
            {
                _context.Gerencias.Remove(gerencia);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult NombreDisponible(string nombre) {
            var nombreExistente = _context.Gerencias.Any(g => g.Nombre == nombre);

            if(!nombreExistente)
            {
                return Json(true);
            }
            else
            {
                return Json(Errors.NombreDuplicadoError);
            }
        }

        private bool GerenciaExists(int id)
        {
          return _context.Gerencias.Any(e => e.Id == id);
        }
    }
}
