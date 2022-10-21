using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP_D.Data;
using ERP_D.Models;

namespace ERP_D.Controllers
{
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
            ViewData["CentroDeCostoId"] = new SelectList(_context.CentrosDeCosto, "Id", "Nombre");
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
        public async Task<IActionResult> Create([Bind("Id,Nombre,EsGerenciaGeneral,GerenciaId,ResponsableId,EmpresaId,CentroDeCostoId")] Gerencia gerencia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gerencia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CentroDeCostoId"] = new SelectList(_context.CentrosDeCosto, "Id", "Nombre", gerencia.CentroDeCostoId);
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "Id", "Nombre", gerencia.GerenciaId);
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Nombre", gerencia.EmpresaId);
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "Id", "Nombre", gerencia.ResponsableId);
            return View(gerencia);
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
            ViewData["CentroDeCostoId"] = new SelectList(_context.CentrosDeCosto, "Id", "Nombre", gerencia.CentroDeCostoId);
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "Id", "Nombre", gerencia.GerenciaId);
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Nombre", gerencia.EmpresaId);
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "Id", "Nombre", gerencia.ResponsableId);
            return View(gerencia);
        }

        // POST: Gerencias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,EsGerenciaGeneral,GerenciaId,ResponsableId,EmpresaId,CentroDeCostoId")] Gerencia gerencia)
        {
            if (id != gerencia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gerencia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GerenciaExists(gerencia.Id))
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
            ViewData["CentroDeCostoId"] = new SelectList(_context.CentrosDeCosto, "Id", "Nombre", gerencia.CentroDeCostoId);
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "Id", "Nombre", gerencia.GerenciaId);
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Nombre", gerencia.EmpresaId);
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "Id", "Nombre", gerencia.ResponsableId);
            return View(gerencia);
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

        private bool GerenciaExists(int id)
        {
          return _context.Gerencias.Any(e => e.Id == id);
        }
    }
}
