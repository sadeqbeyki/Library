using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Areas.adminPanel.Controllers;

[Area("adminPanel")]

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}