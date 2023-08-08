using AppFramework.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Areas.adminPanel.Controllers;

[Area("adminPanel")]
//[Authorize(Roles = "admin, content manager, member")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}