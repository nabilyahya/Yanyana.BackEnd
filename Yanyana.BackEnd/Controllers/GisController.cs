using Microsoft.AspNetCore.Mvc;

namespace Yanyana.BackEnd.Api.Controllers
{
    public class GisController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
