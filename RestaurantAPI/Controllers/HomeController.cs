using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("{controller}/{action}")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public class Param
        {

            public string name { get; set; }
            public string age { get; set; }
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }



        [HttpGet]
        public IActionResult Index()
        {
            return StatusCode(400, "Jest cyc");
        }


        [HttpPost("{p}")]
        public IActionResult Index([FromBody]Param Param, [FromRoute] string p, [FromQuery] string huj)
        {
            return Ok("Jest cyc: " + Param.name + p + huj);
        }

        [HttpGet]
        public IActionResult ASD()
        {
            return Ok("ASD: ");
        }

        [HttpGet("{p}")]
        public IActionResult qwe([FromRoute] int p)
        {
            return Ok("ASD: " + p);
        }
    }
}
