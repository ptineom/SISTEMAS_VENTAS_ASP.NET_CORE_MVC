using AplicacionWeb.SistemaVentas.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Controllers
{
    [Route("[controller]")]
    public class MovieController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(new RegistrationViewModel());
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegistrationViewModel request)
        {
            string mensaje = "";
            if (ModelState.IsValid)
            {
                mensaje = "Todo ok";
            }
            else
            {
                mensaje = "Se encontraron errores en el ModelState";
            }
            return Ok(new { Resultado = true, Message = mensaje });
        }
    }
}
