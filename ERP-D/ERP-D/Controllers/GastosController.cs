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
using ERP_D.ViewModels.Gastos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace ERP_D.Controllers
{
    
    public class GastosController : Controller
    {
        private readonly ErpContext _context;
        private readonly UserManager<Persona> _userManager;

        public GastosController(UserManager<Persona> userManager, ErpContext erpContext)
        {
            this._userManager = userManager;
            this._context = erpContext;
        }

        // GET: Gastos
        [Authorize(Roles = "Admin, Empleado, RH")]
        public IActionResult MisGastos(string sortOrder, string searchEmpleado)
        {
            IQueryable<Gasto> gastos = null;

            var idEmpleado = Int32.Parse(_userManager.GetUserId(User));
            ViewBag.FechaSortParm = sortOrder == "Fecha" ? "fecha_desc" : "Fecha";
            gastos = _context.Gastos.Include(g => g.CentroDeCosto).ThenInclude(c => c.Gerencia).Include(g => g.Empleado).Where(g => g.EmpleadoId == idEmpleado).OrderByDescending(g => g.Fecha);
            switch (sortOrder)
            {
                case "Fecha":
                    gastos = gastos.OrderBy(g => g.Fecha);
                    break;
                case "fecha_desc":
                    gastos = gastos.OrderByDescending(g => g.Fecha);
                    break;
            }

            return View(gastos.ToList());
        }

        [Authorize(Roles = "Admin, RH")]
        public IActionResult Index(string sortOrder, string searchEmpleado)
        {
            IQueryable<Gasto> gastos = null;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.FechaSortParm = sortOrder == "Fecha" ? "fecha_desc" : "Fecha";
            gastos = _context.Gastos.Include(g => g.Empleado).Include(g => g.CentroDeCosto).ThenInclude(c => c.Gerencia).OrderBy(g => g.Empleado.Nombre).ThenBy(g => g.Empleado.Apellido); ;

            if (!String.IsNullOrEmpty(searchEmpleado))
            {
                gastos = gastos.Where(g => g.Empleado.Nombre.Contains(searchEmpleado) 
                                        || g.Empleado.Apellido.Contains(searchEmpleado));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    gastos = gastos.OrderByDescending(g => g.Empleado.Nombre).ThenBy(g => g.Empleado.Apellido);
                    break;
                case "Fecha":
                    gastos = gastos.OrderBy(g => g.Fecha);
                    break;
                case "fecha_desc":
                    gastos = gastos.OrderByDescending(g => g.Fecha);
                    break;
                default:
                    gastos = gastos.OrderByDescending(g => g.Fecha);
                    break;

            }

            return View(gastos.ToList());
        }

        // GET: Gastos/Details/5
        [Authorize(Roles = "Admin, RH")]
        public async Task<IActionResult> Details(String returnUrl, int? id)
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
        [Authorize(Roles = "Empleado")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gastos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Descripcion,Monto")] CrearGastoEmp gastoForm)
        {
            if (ModelState.IsValid)
            {

                var idEmpleado = Int32.Parse(_userManager.GetUserId(User));

                var empleado = await _context.Empleados
                    .Include(e => e.Posicion)
                    .ThenInclude(p => p.Gerencia)
                    .ThenInclude(g => g.CentroDeCosto)
                    .FirstOrDefaultAsync(m => m.Id == idEmpleado);


                if(empleado == null || empleado.Posicion == null)
                {
                    ModelState.AddModelError(String.Empty, "El empleado debe tener asignada una posicion");
                    return View(gastoForm);
                }

                if (empleado.Posicion.Gerencia == null)
                {
                    ModelState.AddModelError(String.Empty, "El empleado debe tener asignada una gerencia");
                    return View(gastoForm);
                }

                if (empleado.Posicion.Gerencia.CentroDeCosto == null)
                {
                    ModelState.AddModelError(String.Empty, "El empleado debe tener asignada un centro de costo");
                    return View(gastoForm);
                }

                if (gastoForm.Monto > empleado.Posicion.Gerencia.CentroDeCosto.MontoMaximo)
                {
                    ModelState.AddModelError(String.Empty, "El monto supera el monto maximo");
                    return View(gastoForm);
                }

                var centroCosto = empleado.Posicion.Gerencia.CentroDeCosto;

                var totalGastos = _context.Gastos
                .Where(g => g.CentroDeCostoId == centroCosto.Id)
                .Sum(g => g.Monto);

                if (gastoForm.Monto + totalGastos > centroCosto.MontoMaximo)
                {
                    ModelState.AddModelError(String.Empty, "El monto del gasto supera el monto máximo permitido");
                    return View(gastoForm);
                }

                var gasto = new Gasto();

                gasto.Fecha = DateTime.Now;
                gasto.Descripcion = gastoForm.Descripcion;
                gasto.Monto = gastoForm.Monto;
                gasto.EmpleadoId = empleado.Id;
                gasto.CentroDeCostoId = (int)empleado.Posicion.Gerencia.CentroDeCostoId;

                _context.Add(gasto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MisGastos));
            }
            return View(gastoForm);
        }

       // GET: Gastos/Edit/5
        [Authorize(Roles = "!")]
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
        [Authorize(Roles = "Admin, RH")]
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
        [Authorize(Roles = "!")]
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
        [Authorize(Roles = "Admin, RH")]
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
