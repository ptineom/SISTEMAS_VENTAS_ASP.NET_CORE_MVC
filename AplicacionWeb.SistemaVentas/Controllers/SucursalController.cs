using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Controllers
{
    [Route("controller")]
    public class SucursalController : Controller
    {
        [Route("[action]")]
        [AllowAnonymous]
        public IActionResult Index()
        {

            return View();
        }
    }
}
