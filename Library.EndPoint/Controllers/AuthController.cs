using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
