using ERP_D.Data;
using ERP_D.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace ERP_D
{
    public static class StartUp
    {
        private static void ConfigureServices (WebApplicationBuilder builder)
        {
            //builder.Services.AddDbContext<ErpContext>(options => options.UseInMemoryDatabase("ErpDB"));            
            builder.Services.AddDbContext<ErpContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ErpDBCS")));

            builder.Services.AddIdentity<Persona, Rol>().AddEntityFrameworkStores<ErpContext>();

            //Password por defecto va a ser Password1!

            builder.Services.Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 4;

                }
                
            );

            builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
                options =>
                {
                    options.LoginPath = "/Account/IniciarSesion";
                    options.AccessDeniedPath = "/Account/AccesoDenegado";
                    options.Cookie.Name = "IdentidadERP";
                }
            );
            
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

            using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var contexto = serviceScope.ServiceProvider.GetRequiredService<ErpContext>();

                contexto.Database.Migrate();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

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
