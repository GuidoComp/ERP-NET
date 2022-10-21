using ERP_D.Data;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace ERP_D
{
    public static class StartUp
    {
        private static void ConfigureServices (WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ErpContext>(options => options.UseInMemoryDatabase("ErpDB"));            
            builder.Services.AddControllersWithViews();
                        
        }

        private static void Configure (WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }

        public static WebApplication InicializarApp(String[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            var app = builder.Build();
            Configure(app);
            return app;
        }
    }
}
