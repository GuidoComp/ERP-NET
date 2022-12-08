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
using Microsoft.Data.SqlClient;
using ERP_D.ViewModels.Posiciones;

namespace ERP_D.Controllers
{
    [Authorize(Roles = "Admin, RH")]
    public class PosicionesController : Controller
    {
        private readonly ErpContext _context;

        public PosicionesController(ErpContext context)
        {
            _context = context;
        }

        // GET: Posiciones
        public async Task<IActionResult> Index()
        {
            var erpContext = _context.Posiciones.Include(p => p.Empleado).Include(p => p.Gerencia).Include(p => p.Responsable);
            
            return View(await erpContext.ToListAsync());
        }

        // GET: Posiciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posiciones == null)
            {
                return NotFound();
            }

            var posicion = await _context.Posiciones
                .Include(p => p.Empleado)
                .Include(p => p.Gerencia)
                .Include(p => p.Responsable)
                .Include(p => p.Subordinadas)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (posicion == null)
            {
                return NotFound();
            }

            return View(posicion);
        }

        // GET: Posiciones/Create
        public IActionResult Create()
        {
            //ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido");
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias.Include(g => g.Empresa), "Id", "ObtenerEmpresaNombre");
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "Id", "Nombre");
            ViewBag.AnyGerenciaGeneral = _context.Posiciones.Any(p => p.ResponsableId == null);
            return View();
        }

        // POST: Posiciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Sueldo,ResponsableId,GerenciaId")] Posicion posicion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(posicion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AnyGerenciaGeneral = _context.Posiciones.Any(p => p.ResponsableId == null);
            //ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido");
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "Id", "Nombre", posicion.GerenciaId);
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "Id", "Nombre", posicion.ResponsableId);
            return View(posicion);
        }

        // GET: Posiciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posiciones == null)
            {
                return NotFound();
            }

            var posicion = await _context.Posiciones.FindAsync(id);
            if (posicion == null)
            {
                return NotFound();
            }

            if(posicion.ResponsableId != null) 
            {
                ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "Id", "Nombre", posicion.ResponsableId);
            }

            var editPosicion = new EditPosicion();
            editPosicion.Id = posicion.Id;
            editPosicion.Nombre = posicion.Nombre;
            editPosicion.Descripcion = posicion.Descripcion;
            editPosicion.Sueldo = posicion.Sueldo;
            editPosicion.ResponsableId = posicion.ResponsableId;
            editPosicion.GerenciaId = posicion.GerenciaId;
            //editPosicion.InfoGerenciaYEmpresa = posicion.InfoGerenciaYEmpresa;

            //ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido");
            //ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "Id", "Nombre", posicion.GerenciaId);
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias.Include(g => g.Empresa), "Id", "ObtenerEmpresaNombre");

            return View(editPosicion);
        }

        // POST: Posiciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Sueldo,ResponsableId,GerenciaId")] EditPosicion posicionForm)
        {
            if (id != posicionForm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var posicionEnDb = _context.Posiciones.Find(posicionForm.Id);
                    if (posicionEnDb == null)
                    {
                        return NotFound();
                    }
                    posicionEnDb.Nombre = posicionForm.Nombre;
                    posicionEnDb.Descripcion = posicionForm.Descripcion;
                    posicionEnDb.Sueldo = posicionForm.Sueldo;
                    //posicionEnDb.EmpleadoId = posicionForm.EmpleadoId;
                    posicionEnDb.ResponsableId = posicionForm.ResponsableId;
                    posicionEnDb.GerenciaId = posicionForm.GerenciaId;
                    //var empresa = posicionEnDb.InfoGerenciaYEmpresa.Split('-')[0];
                    //var gerencia = _context.Gerencias.Find(posicionForm.GerenciaId).Nombre;
                    //posicionEnDb.InfoGerenciaYEmpresa = $"{empresa}- {gerencia}";

                    _context.Posiciones.Update(posicionEnDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PosicionExists(posicionForm.Id))
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
            //ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido");
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "Id", "Nombre", posicionForm.GerenciaId);
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "Id", "Nombre", posicionForm.ResponsableId);
            return View(posicionForm);
        }

        // GET: Posiciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posiciones == null)
            {
                return NotFound();
            }

            var posicion = await _context.Posiciones
                .Include(p => p.Empleado)
                .Include(p => p.Gerencia)
                .Include(p => p.Responsable)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (posicion == null)
            {
                return NotFound();
            }

            return View(posicion);
        }

        // POST: Posiciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posiciones == null)
            {
                return Problem("Entity set 'ErpContext.Posiciones'  is null.");
            }
            var posicion = await _context.Posiciones.FindAsync(id);
            if (posicion != null)
            {
                _context.Posiciones.Remove(posicion);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                SqlException innerException = e.InnerException as SqlException;
                if (innerException != null && (innerException.Number == 547))
                {
                    return RedirectToAction(nameof(ErrorView));
                }
            }
            
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult NombreDisponible(string nombre)
        {
            var nombreExistente = _context.Posiciones.Any(g => g.Nombre == nombre);

            if (!nombreExistente)
            {
                return Json(true);
            }
            else
            {
                return Json(Errors.NombreDuplicadoError);
            }
        }

        public IActionResult ErrorView()
        {
            return View();
        }

        private bool PosicionExists(int id)
        {
            return _context.Posiciones.Any(e => e.Id == id);
        }
    }
}
