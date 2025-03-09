using Microsoft.AspNetCore.Mvc;

namespace Yanyana.BackEnd.Api.Controllers
{
    public class InteractionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
