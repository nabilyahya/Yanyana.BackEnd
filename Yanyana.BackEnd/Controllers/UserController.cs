using Microsoft.AspNetCore.Mvc;

namespace Yanyana.BackEnd.Api.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
