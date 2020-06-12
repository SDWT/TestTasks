using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Koshelek.TestTask.Client.Models;

namespace Koshelek.TestTask.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Clients controller
        /// </summary>
        /// <param name="logger">logger</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
