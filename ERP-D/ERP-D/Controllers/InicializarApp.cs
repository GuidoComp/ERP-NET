using ERP_D.Models;
using ERP_D.Data;
using Microsoft.AspNetCore.Mvc;

namespace ERP_D.Controllers
{
    public class InicializarApp : Controller
    {
        private readonly ErpContext _context;

        public InicializarApp(ErpContext context)
        {
            this._context = context;
        }

        public IActionResult Inicializar()
        {
            CrearPosiciones();
            return RedirectToAction("Index", "Posiciones");
        }

        private void CrearPosiciones()
        {
            Posicion posicion = new Posicion()
            {
                Nombre = "Gerente general",
                Sueldo = 2000,


            };
            _context.Posiciones.Add(posicion);
            _context.SaveChanges();

        }
    }
}

