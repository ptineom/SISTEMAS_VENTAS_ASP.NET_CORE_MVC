using AplicacionWeb.SistemaVentas.Services.Security.Contracts;
using AplicacionWeb.SistemaVentas.Services.Security.Implementations;
using CapaDao.Contracts;
using CapaDao.Implementations;
using CapaNegocio.Contracts;
using CapaNegocio.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Register
{
    public static class IocRegister
    {
        public static void AddRegistration(this IServiceCollection services)
        {
            AddRegisterConnection(services);
            AddRegisterOthers(services);
            AddRegisterCapaNegocio(services);
            AddRegisterCapaDao(services);
        }
        public static void AddRegisterCapaNegocio(IServiceCollection services)
        {
            services.AddTransient<IProveedorService, ProveedorService>();
            services.AddTransient<IRefreshTokenService, RefreshTokenService>();
            services.AddTransient<ISucursalUsuarioService, SucursalUsuarioService>();
            services.AddTransient<ISecurityService, SecurityService>();
            services.AddTransient<ICajaAperturaService, CajaAperturaService>();
            services.AddTransient<ICajaService, CajaService>();
            services.AddTransient<IMonedaService, MonedaService>();
            services.AddTransient<ISucursalCajaUsuarioService, SucursalCajaUsuarioService>();
            services.AddTransient<IDocCompraService, DocCompraService>();
        }
        public static void AddRegisterCapaDao(IServiceCollection services)
        {
            services.AddTransient<IProveedorRepository, ProveedorRepository>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddTransient<ISucursalUsuarioRepository, SucursalUsuarioRepository>();
            services.AddTransient<ISecurityRepository, SecurityRepository>();
            services.AddTransient<ICajaAperturaRepository, CajaAperturaRepository>();
            services.AddTransient<ICajaRepository, CajaRepository>();
            services.AddTransient<IMonedaRepository, MonedaRepository>();
            services.AddTransient<ISucursalCajaUsuarioRepository, SucursalCajaUsuarioRepository>();
            services.AddTransient<IDocCompraRepository, DocCompraRepository>();
        }
        public static void AddRegisterConnection(IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var sqlConnectionSb = new SqlConnectionStringBuilder()
            {
                UserID = configuration.GetSection("ConnectionStrings:UserId").Value,
                Password = configuration.GetSection("ConnectionStrings:Password").Value,
                DataSource = configuration.GetSection("ConnectionStrings:DataSource").Value,
                InitialCatalog = configuration.GetSection("ConnectionStrings:InitialCatalog").Value,
                PersistSecurityInfo = true
            };

            //Configurar el proveedor, en este caso SQL Server.
            services.AddScoped<IDbConnection>(db => new SqlConnection(sqlConnectionSb.ConnectionString));
            //La conección será según el proveedor configurado.
            services.AddTransient<IConnection, Connection>();
        }

        public static void AddRegisterOthers(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISessionIdentity, SessionIdentity>();
            services.AddScoped<ITokenProcess, TokenProcess>();
        }
    }
}
