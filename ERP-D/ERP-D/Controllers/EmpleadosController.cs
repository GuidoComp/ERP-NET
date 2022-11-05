using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP_D.Data;
using ERP_D.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;
using ERP_D.ViewModels;

namespace ERP_D.Controllers
{
    [Authorize(Roles = "Admin, Empleado, RH")]
    public class EmpleadosController : Controller
    {
        private readonly ErpContext _context;
        private readonly UserManager<Persona> _userManager;

        public EmpleadosController(UserManager<Persona> userManager, ErpContext erpContext)
        {
            this._userManager = userManager;
            this._context = erpContext;
        }

        // GET: Empleados
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Empleado") && !User.IsInRole("RH"))
            {
                return RedirectToAction(nameof(Details), new { id = Int32.Parse(_userManager.GetUserId(User)) } );
            }
              return View(await _context.Empleados.ToListAsync());
        }

        // GET: Empleados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.Include(e => e.Posicion).FirstOrDefaultAsync(m => m.Id == id);
            
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleados/Create
        [Authorize(Roles = "Admin, RH")]
        public IActionResult Create()
        {
            ViewData["PosicionId"] = new SelectList(_context.Posiciones.Include(p => p.Empleado).Where(p => p.Empleado == null || p.Empleado.EmpleadoActivo == false), "Id", "Nombre");
            return View();
        }

        // POST: Empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, RH")]
        public async Task<IActionResult> Create([Bind("ObraSocial,EmpleadoActivo,Foto,Id,DNI,Nombre,Apellido,Direccion,PosicionId, RH, TipoTelefono, NumeroTelefono")] CreacionEmpleado empleadoForm)
        {
            IdentityResult resultado = null;
            if (ModelState.IsValid)
            {
                var nuevoEmpleado = new Empleado();

                var fotoPath = await CrearFoto(empleadoForm.Foto, empleadoForm.Nombre + empleadoForm.Apellido);

                nuevoEmpleado.Nombre = empleadoForm.Nombre;
                nuevoEmpleado.DNI = empleadoForm.DNI;
                nuevoEmpleado.Apellido = empleadoForm.Apellido;
                nuevoEmpleado.Foto = fotoPath;
                nuevoEmpleado.EmpleadoActivo = empleadoForm.EmpleadoActivo;
                nuevoEmpleado.Direccion = empleadoForm.Direccion;
                nuevoEmpleado.ObraSocial = empleadoForm.ObraSocial;
                nuevoEmpleado.UserName = empleadoForm.DNI.ToString();
                nuevoEmpleado.PosicionId = empleadoForm.PosicionId;
                nuevoEmpleado.Email = empleadoForm.Nombre.ToLower() + "." + empleadoForm.Apellido.ToLower() + "@ort.edu.ar";

                    resultado = await _userManager.CreateAsync(nuevoEmpleado, nuevoEmpleado.DNI.ToString());
                    if (resultado.Succeeded)
                    {
                        if (empleadoForm.RH)
                        {
                            await _userManager.AddToRoleAsync(nuevoEmpleado, "Empleado");
                            await _userManager.AddToRoleAsync(nuevoEmpleado, "RH");
                        }else
                        {
                            await _userManager.AddToRoleAsync(nuevoEmpleado, "Empleado");
                        }
                        if(!empleadoForm.TipoTelefono.ToString().Equals("") && empleadoForm.NumeroTelefono != null)
                        {
                            _context.Telefonos.Add(new Telefono {Tipo=empleadoForm.TipoTelefono, Numero=empleadoForm.NumeroTelefono, PersonaId=nuevoEmpleado.Id});
                            await _context.SaveChangesAsync();
                        }
                        return RedirectToAction(nameof(Index));
                    }               
            }

            if(resultado != null)
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);

                }
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Erorr inesperado");
            }

            ViewData["PosicionId"] = new SelectList(_context.Posiciones, "Id", "Nombre", empleadoForm.PosicionId);
            return View(empleadoForm);
        }

        // GET: Empleados/Edit/5
        [Authorize(Roles = "Admin, RH")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.Include(e => e.Posicion).FirstOrDefaultAsync(e => e.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            var empleadoEdit = new CreacionEmpleado();

            empleadoEdit.DNI = empleado.DNI;
            empleadoEdit.Nombre = empleado.Nombre;
            empleadoEdit.Apellido = empleado.Apellido;
            empleadoEdit.ObraSocial = empleado.ObraSocial;
            empleadoEdit.EmpleadoActivo = empleado.EmpleadoActivo;
            // TODO: crearEmpleado.Foto = empleado.Foto;
            empleadoEdit.Direccion = empleado.Direccion;
            empleadoEdit.PosicionId = empleado.PosicionId;

            List<Posicion> positionList = _context.Posiciones.Include(p => p.Empleado).Where(p => p.Empleado == null).ToList();

            positionList.Add(empleado.Posicion);
            //positionList.Add(null);

            // TODO: Como resolvemos aca al editar posicion????
            ViewData["PosicionId"] = new SelectList(positionList, "Id", "Nombre", empleado.PosicionId);
            return View(empleadoEdit);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, RH")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ObraSocial,EmpleadoActivo,Foto,Id,DNI,Nombre,Apellido,Direccion,PosicionId")] CreacionEmpleado empleadoForm)
        {
            if (id != empleadoForm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var empleadoDB = _context.Empleados.Find(empleadoForm.Id);

                    if(empleadoDB == null)
                    {
                        return NotFound();
                    }

                    empleadoDB.DNI = empleadoForm.DNI;
                    empleadoDB.Nombre = empleadoForm.Nombre;
                    empleadoDB.Apellido = empleadoForm.Apellido;
                    empleadoDB.ObraSocial = empleadoForm.ObraSocial;
                    // TODO: empleadoDB.Foto = empleadoForm.Foto;
                    empleadoDB.Direccion = empleadoForm.Direccion;
                    empleadoDB.PosicionId = empleadoForm.PosicionId;

                    _context.Update(empleadoDB);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleadoForm.Id))
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
            ViewData["PosicionId"] = new SelectList(_context.Posiciones, "Id", "Nombre", empleadoForm.PosicionId);
            return View(empleadoForm);
        }

        // Action para que el rolm de empleado modifique sus datos
        public async Task<IActionResult> PersonalEdit(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            var empleadoEdit = new PersonalEdit();
            empleadoEdit.Id = empleado.Id;
            empleadoEdit.Direccion = empleado.Direccion;
            empleadoEdit.Foto = empleado.Foto;

            return View(empleadoEdit);
        }

        // POST: Empleados/Edit/5
        // Action para que el rolm de empleado modifique sus datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PersonalEdit(int id, [Bind("Id, Foto, Direccion")] PersonalEdit empleadoForm)
        {
            if (id != empleadoForm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var empleadoDB = _context.Empleados.Find(empleadoForm.Id);

                    if (empleadoDB == null)
                    {
                        return NotFound();
                    }

                    empleadoDB.Foto = empleadoForm.Foto;
                    empleadoDB.Direccion = empleadoForm.Direccion;

                    _context.Update(empleadoDB);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleadoForm.Id))
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
            return View(empleadoForm);
        }


        // GET: Empleados/Delete/5
        [Authorize(Roles = "Admin, RH")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, RH")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Empleados == null)
            {
                return Problem("Entity set 'ErpContext.Empleados'  is null.");
            }
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado != null)
            {
                empleado.EmpleadoActivo = false;
                _context.Empleados.Update(empleado);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> CrearFoto(IFormFile foto, String nombreFoto)
        {
            var usePath = "";
            if (foto != null && foto.Length > 0)
            {
                var nuevaImagen = new Imagen();
                var fileName = nombreFoto + ".jpg";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                usePath = "/images/" + fileName;
                nuevaImagen.Path = usePath;
                nuevaImagen.Nombre = fileName;
                _context.Imagenes.Add(nuevaImagen);
                _context.SaveChanges();
                using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                {
                    await foto.CopyToAsync(fileSrteam);
                }
            }
            return usePath;
        }
        private bool EmpleadoExists(int id)
        {
          return _context.Empleados.Any(e => e.Id == id);
        }
    }
}
