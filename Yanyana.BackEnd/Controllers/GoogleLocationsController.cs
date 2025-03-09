using Microsoft.AspNetCore.Mvc;

namespace Yanyana.BackEnd.Api.Controllers
{
    public class GoogleLocationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
