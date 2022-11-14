using ERP_D.Data;
using ERP_D.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP_D.Helpers
{
    public static class Utils
    {
        public static async Task<string> CrearFoto(IFormFile foto, String nombreFoto, ErpContext _context)
        {
            var usePath = "";

            if (nombreFoto == null || nombreFoto == "")
            {
                nombreFoto = Path.GetFileName(foto.FileName);
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
    }
}
