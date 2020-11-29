using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using ErrorViewModel = Demo.UI.Models.ErrorViewModel;

namespace Demo.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            _logger.LogInformation($"Load Home Page");
            return View();
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation($"Load Privacy Page");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorViewModel = new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier};
            var exceptionPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var ex = exceptionPathFeature?.Error;
            if (ex == null || !ex.Data.Contains("API Route")) return View(errorViewModel);
            errorViewModel.ApiRoute = ex.Data["API Route"]?.ToString();
            errorViewModel.ApiStatus = ex.Data["API Status"]?.ToString();
            errorViewModel.ApiErrorId = ex.Data["API ErrorId"]?.ToString();
            errorViewModel.ApiTitle = ex.Data["API Title"]?.ToString();
            errorViewModel.ApiDetail = ex.Data["API Detail"]?.ToString();
            return View(errorViewModel);
        }
    }
}
