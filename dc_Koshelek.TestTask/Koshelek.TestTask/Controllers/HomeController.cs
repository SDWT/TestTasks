using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Koshelek.TestTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Sender");
        }

        public IActionResult Sender()
        {
            _logger.LogInformation("Open Sender browser client");
            return View();
        }

        public IActionResult Recipient()
        {
            _logger.LogInformation("Open Recipient browser client");
            return View();
        }

        public IActionResult Journal()
        {
            _logger.LogInformation("Open Journal browser client");
            return View();
        }
    }
}
