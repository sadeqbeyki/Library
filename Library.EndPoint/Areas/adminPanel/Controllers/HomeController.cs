using Library.EndPoint.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Library.EndPoint.Areas.adminPanel.Controllers;

[Area("adminPanel")]

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}