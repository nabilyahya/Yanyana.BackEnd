using Microsoft.AspNetCore.Mvc;

namespace Yanyana.BackEnd.Api.Controllers
{
    public class PlacesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
