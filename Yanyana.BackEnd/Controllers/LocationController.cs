using Microsoft.AspNetCore.Mvc;

namespace Yanyana.BackEnd.Api.Controllers
{
    public class LocationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
