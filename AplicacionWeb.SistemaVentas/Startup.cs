using CapaNegocio;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AplicacionWeb.SistemaVentas.CustomValidation;
using AplicacionWeb.SistemaVentas.Hubs;
using AplicacionWeb.SistemaVentas.Register;
using AplicacionWeb.SistemaVentas.Services.Security.Contracts;
using AplicacionWeb.SistemaVentas.Services.Security.Implementations;

namespace AplicacionWeb.SistemaVentas
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ////Por defecto el api, retorna los atributos del json en LowercamelCase, pero configuremos para que respete los nombres de los atributos del json.
            //services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    //options.Cookie.SecurePolicy = _environment.IsDevelopment() ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
                    options.LoginPath = "/Login/Index";
                    options.Cookie.Name = ".AspNetCore.Cookies.SistemaVentas";
                    options.SlidingExpiration = true;
                });

            services.AddControllersWithViews();

            //Agregando Ioc configurados.
            services.AddRegistration();

            services.AddHttpContextAccessor();

            services.AddTransient<IResultadoOperacion, ResultadoOperacion>();
            services.AddScoped<ISessionIdentity, SessionIdentity>();

            services.AddRazorPages().AddMvcOptions(options => {
                options.MaxModelValidationErrors = 50;
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor( _ => "The field is required.");
            });

            //Validaciones personalizadas
            services.AddSingleton<IValidationAttributeAdapterProvider, AdapterProvider>();

            services.AddSignalR();

      
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<CambiarEstadoCajaHub>("/cambiarestadocajahub");
            });


        }
    }
}
