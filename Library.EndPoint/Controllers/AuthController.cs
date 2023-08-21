using LibIdentity.Domain.UserAgg;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Controllers
{
    public class AuthController : Controller
    {
        public static UserIdentity user = new UserIdentity();
        public IActionResult Index()
        {
            return View();
        }
    }
}
