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
    public class GastosController : Controller
    {
        private readonly ErpContext _context;

        public GastosController(ErpContext context)
        {
            _context = context;
        }

        // GET: Gastos
        public async Task<IActionResult> Index()
        {
            var erpContext = _context.Gastos.Include(g => g.CentroDeCosto).Include(g => g.Empleado);
            return View(await erpContext.ToListAsync());
        }

        // GET: Gastos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Gastos == null)
            {
                return NotFound();
            }

            var gasto = await _context.Gastos
                .Include(g => g.CentroDeCosto)
                .Include(g => g.Empleado)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gasto == null)
            {
                return NotFound();
            }

            return View(gasto);
        }

        // GET: Gastos/Create
        public IActionResult Create()
        {
            ViewData["CentroDeCostoId"] = new SelectList(_context.CentrosDeCosto, "Id", "Nombre");
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", "Nombre");
            return View();
        }

        // POST: Gastos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,Monto,Fecha,EmpleadoId,CentroDeCostoId")] Gasto gasto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gasto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CentroDeCostoId"] = new SelectList(_context.CentrosDeCosto, "Id", "Nombre", gasto.CentroDeCostoId);
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", gasto.EmpleadoId);
            return View(gasto);
        }

        // GET: Gastos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Gastos == null)
            {
                return NotFound();
            }

            var gasto = await _context.Gastos.FindAsync(id);
            if (gasto == null)
            {
                return NotFound();
            }
            ViewData["CentroDeCostoId"] = new SelectList(_context.CentrosDeCosto, "Id", "Nombre", gasto.CentroDeCostoId);
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", gasto.EmpleadoId);
            return View(gasto);
        }

        // POST: Gastos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,Monto,Fecha,EmpleadoId,CentroDeCostoId")] Gasto gasto)
        {
            if (id != gasto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gasto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GastoExists(gasto.Id))
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
            ViewData["CentroDeCostoId"] = new SelectList(_context.CentrosDeCosto, "Id", "Nombre", gasto.CentroDeCostoId);
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", gasto.EmpleadoId);
            return View(gasto);
        }

        // GET: Gastos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Gastos == null)
            {
                return NotFound();
            }

            var gasto = await _context.Gastos
                .Include(g => g.CentroDeCosto)
                .Include(g => g.Empleado)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gasto == null)
            {
                return NotFound();
            }

            return View(gasto);
        }

        // POST: Gastos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Gastos == null)
            {
                return Problem("Entity set 'ErpContext.Gastos'  is null.");
            }
            var gasto = await _context.Gastos.FindAsync(id);
            if (gasto != null)
            {
                _context.Gastos.Remove(gasto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GastoExists(int id)
        {
          return _context.Gastos.Any(e => e.Id == id);
        }
    }
}
