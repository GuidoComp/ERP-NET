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
using ERP_D.ViewModels;
using System.Xml.Schema;

namespace ERP_D.Controllers
{
    [Authorize(Roles = "Admin, RH")]
    public class CentrosDeCostoController : Controller
    {
        private readonly ErpContext _context;

        public CentrosDeCostoController(ErpContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<CentroCostoLista> listaCentros = new List<CentroCostoLista>();
            var centros = _context.CentrosDeCosto.Include(c => c.Gerencia).Include(c => c.Gastos);


            foreach (var centro in centros)
            {
                double suma = 0;
                var centroItem = new CentroCostoLista();
                centroItem.Id = centro.Id;
                centroItem.Nombre = centro.Nombre;
                centroItem.MontoMaximo = centro.MontoMaximo;
                centroItem.Gerencia = centro.Gerencia.Nombre;

                foreach (var gasto in centro.Gastos)
                {
                    suma = suma + gasto.Monto;
                }
                centroItem.Total = suma;

                listaCentros.Add(centroItem);
            }

              return View(listaCentros);
        }

        // GET: CentroDeCostos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CentrosDeCosto == null)
            {
                return NotFound();
            }

            var centroDeCosto = await _context.CentrosDeCosto
                .Include(c => c.Gastos.OrderByDescending(g => g.Monto))
                .ThenInclude(g => g.Empleado)
                .FirstOrDefaultAsync(m => m.Id == id)
                ;
            
            if (centroDeCosto == null)
            {
                return NotFound();
            }

            double total = 0;

            List<Gasto> gastos = centroDeCosto.Gastos.ToList();

            for(int i = 0; i < gastos.Count; i++)
            {
                total = total + gastos[i].Monto;
            }

            ViewData["TotalAcumulado"] = total;

            return View(centroDeCosto);
        }

        // GET: CentroDeCostos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CentrosDeCosto == null)
            {
                return NotFound();
            }

            var centroDeCosto = await _context.CentrosDeCosto.FindAsync(id);
            if (centroDeCosto == null)
            {
                return NotFound();
            }
            return View(centroDeCosto);
        }

        // POST: CentroDeCostos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,MontoMaximo")] CentroDeCosto centroDeCosto)
        {
            if (id != centroDeCosto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(centroDeCosto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CentroDeCostoExists(centroDeCosto.Id))
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
            return View(centroDeCosto);
        }

        // GET: CentroDeCostos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CentrosDeCosto == null)
            {
                return NotFound();
            }

            var centroDeCosto = await _context.CentrosDeCosto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (centroDeCosto == null)
            {
                return NotFound();
            }

            return View(centroDeCosto);
        }

        // POST: CentroDeCostos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CentrosDeCosto == null)
            {
                return Problem("Entity set 'ErpContext.CentrosDeCosto'  is null.");
            }
            var centroDeCosto = await _context.CentrosDeCosto.FindAsync(id);
            if (centroDeCosto != null)
            {
                _context.CentrosDeCosto.Remove(centroDeCosto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CentroDeCostoExists(int id)
        {
          return _context.CentrosDeCosto.Any(e => e.Id == id);
        }
    }
}
