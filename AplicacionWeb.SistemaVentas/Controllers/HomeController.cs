using AplicacionWeb.SistemaVentas.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _accessor;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _configuration = configuration;
            _accessor = accessor;
        }

        [Authorize]
        //[Route("Index/{id}")]
        public IActionResult Index()
        {
            var gg = HttpContext.User.Identity;
            var hh = HttpContext.User.Identity.IsAuthenticated;
            ClaimsIdentity identity = (ClaimsIdentity)HttpContext.User.Identity;

            var ggggg = _accessor.HttpContext.User.Identity.IsAuthenticated; 

            var PASSWORD_EMAIL_ORIGIN = _configuration["PASSWORD_EMAIL_ORIGIN"];
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var access = await this.HttpContext.GetTokenAsync("");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
