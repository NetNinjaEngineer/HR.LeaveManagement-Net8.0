using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HR.LeaveManagement.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClient client;

        public HomeController(ILogger<HomeController> logger, IClient client)
        {
            _logger = logger;
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            var leaveTypes = await this.client.LeaveTypesAllAsync();
            return View(leaveTypes);
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
    }
}
