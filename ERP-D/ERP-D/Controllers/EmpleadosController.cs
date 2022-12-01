using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP_D.Data;
using ERP_D.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;
using ERP_D.ViewModels;
using ERP_D.ViewModels.Empleados;
using ERP_D.Helpers;

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

        [Authorize(Roles = "Admin, RH")]
        public async Task<IActionResult> Reactivar(int id)
        {
            var empleado = _context.Empleados.Find(id);

            if (empleado == null)
            {
                return NotFound();
            }

            if (!empleado.EmpleadoActivo)
            {
                empleado.EmpleadoActivo = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // GET: Empleados
        [Authorize(Roles = "Admin, Empleado, RH")]
        public async Task<IActionResult> Index(string sortOrder)
        {
            if (User.IsInRole("Empleado") && !User.IsInRole("RH"))
            {
                return RedirectToAction(nameof(Details), new { id = Int32.Parse(_userManager.GetUserId(User)) });
            }

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SalarioSortParm = sortOrder == "Salario" ? "salario_desc" : "Salario";

            var empleados = _context.Empleados.Include(e => e.Posicion).OrderBy(e => e.Nombre).ThenBy(e => e.Apellido);
            switch (sortOrder)
            {
                case "name_desc":
                    empleados = empleados.OrderByDescending(e => e.Nombre).ThenBy(e => e.Apellido);
                    break;
                case "Salario":
                    empleados = empleados.OrderBy(e => e.Posicion.Sueldo);
                    break;
                case "salario_desc":
                    empleados = empleados.OrderByDescending(e => e.Posicion.Sueldo);
                    break;
                //default:
                //    empleados = empleados.OrderBy(e => e.Nombre).ThenBy(e => e.Apellido);
                //    break;
            }

            return View(empleados.ToList());
        }

        // GET: Empleados/Details/5
        [Authorize(Roles = "Admin, Empleado, RH")]
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
            ViewData["PosicionId"] = new SelectList(_context.Posiciones.Include(p => p.Empleado).Where(p => p.Empleado == null), "Id", "Nombre");
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
                    } else
                    {
                        await _userManager.AddToRoleAsync(nuevoEmpleado, "Empleado");
                    }
                    if (!empleadoForm.TipoTelefono.ToString().Equals("") && empleadoForm.NumeroTelefono != null)
                    {
                        _context.Telefonos.Add(new Telefono { Tipo = empleadoForm.TipoTelefono, Numero = empleadoForm.NumeroTelefono, PersonaId = nuevoEmpleado.Id });
                        await _context.SaveChangesAsync();
                    }
                    return RedirectToAction(nameof(Index));
                }
            }

            if (resultado != null)
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

            var empleado = await _context.Empleados.Include(e => e.Posicion).Include(e => e.Telefonos).FirstOrDefaultAsync(e => e.Id == id);
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
            empleadoEdit.Direccion = empleado.Direccion;
            empleadoEdit.PosicionId = (int)empleado.PosicionId;
            if(empleado.Telefonos != null && empleado.Telefonos.Count > 0)
            {
                empleadoEdit.TipoTelefono = empleado.Telefonos[0].Tipo;
                empleadoEdit.NumeroTelefono = empleado.Telefonos[0].Numero;
            }


            List<Posicion> positionList = _context.Posiciones.Include(p => p.Empleado).Where(p => p.Empleado == null).ToList();

            positionList.Add(empleado.Posicion);

            ViewData["PosicionId"] = new SelectList(positionList, "Id", "Nombre", empleado.PosicionId);
            return View(empleadoEdit);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, RH")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ObraSocial,EmpleadoActivo,Foto,Id,DNI,Nombre,Apellido,Direccion,PosicionId,TipoTelefono,NumeroTelefono")] CreacionEmpleado empleadoForm)
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

                    empleadoDB.DNI = empleadoForm.DNI;
                    empleadoDB.Nombre = empleadoForm.Nombre;
                    empleadoDB.Apellido = empleadoForm.Apellido;
                    empleadoDB.ObraSocial = empleadoForm.ObraSocial;
                    empleadoDB.Direccion = empleadoForm.Direccion;
                    empleadoDB.PosicionId = empleadoForm.PosicionId;
                    empleadoDB.EmpleadoActivo = empleadoForm.EmpleadoActivo;


                    if (empleadoForm.Foto != null )
                    {
                        var pathFoto = await CrearFoto(empleadoForm.Foto, "");

                        empleadoDB.Foto = pathFoto;
                    }

                    if (empleadoForm.NumeroTelefono != null)
                    {
                        Telefono telefonoEdit;
                        if (empleadoDB.Telefonos != null && empleadoDB.Telefonos.Count > 0)
                        {
                            telefonoEdit = empleadoDB.Telefonos[0];
                        }
                        else
                        {
                            telefonoEdit = new Telefono();
                        }

                        telefonoEdit.Tipo = empleadoForm.TipoTelefono;
                        telefonoEdit.Numero = empleadoForm.NumeroTelefono;
                        telefonoEdit.PersonaId = empleadoForm.Id;

                        _context.Telefonos.Update(telefonoEdit);
                        await _context.SaveChangesAsync();
                    }

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
        [Authorize(Roles = "Admin, Empleado, RH")]
        public async Task<IActionResult> PersonalEdit(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.Include(e => e.Telefonos).FirstOrDefaultAsync(e => e.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            var empleadoEdit = new PersonalEdit();
            empleadoEdit.Id = empleado.Id;
            if(empleado.Telefonos.Count > 0)
            {
                empleadoEdit.TipoTelefono = empleado.Telefonos[0].Tipo;
                empleadoEdit.NumeroTelefono = empleado.Telefonos[0].Numero;

            } 
            return View(empleadoEdit);
        }

        // POST: Empleados/Edit/5
        // Action para que el rolm de empleado modifique sus datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Empleado, RH")]
        public async Task<IActionResult> PersonalEdit(int id, [Bind("Id,NombreFoto, Foto, TipoTelefono, NumeroTelefono")] PersonalEdit empleadoForm)
        {
            if (id != empleadoForm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Telefono telefonoEdit;

                    var empleadoDB = await _context.Empleados.Include(e => e.Telefonos).FirstOrDefaultAsync(e => e.Id == empleadoForm.Id) ;

                    if (empleadoDB == null)
                    {
                        return NotFound();
                    }

                    if(empleadoForm.Foto != null)
                    {
                        var pathFoto = await CrearFoto(empleadoForm.Foto, empleadoForm.NombreFoto);

                        empleadoDB.Foto = pathFoto;

                        _context.Update(empleadoDB);

                        await _context.SaveChangesAsync();
                    }

                    if(empleadoForm.NumeroTelefono != null)
                    {
                        if (empleadoDB.Telefonos.Count > 0)
                        {
                            telefonoEdit = empleadoDB.Telefonos[0];
                        }
                        else
                        {
                            telefonoEdit = new Telefono();
                        }

                        telefonoEdit.Tipo = empleadoForm.TipoTelefono;
                        telefonoEdit.Numero = empleadoForm.NumeroTelefono;
                        telefonoEdit.PersonaId = empleadoForm.Id;

                        _context.Telefonos.Update(telefonoEdit);
                        await _context.SaveChangesAsync();
                    }

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
                empleado.PosicionId = null;
                _context.Empleados.Update(empleado);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> CrearFoto(IFormFile foto, String nombreFoto)
        {
            var usePath = "";

            if(nombreFoto == null || nombreFoto == "")
            {
                nombreFoto = foto.FileName;
            }

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
