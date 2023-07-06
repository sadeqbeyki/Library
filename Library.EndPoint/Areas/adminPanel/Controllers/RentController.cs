using LI.ApplicationContracts.UserContracts;
using LMS.Contracts.Rent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.EndPoint.Areas.adminPanel.Controllers;
[Area("adminPanel")]
public class RentController : Controller
{
    public RentSearchModel SearchModel;
    public SelectList Accounts;
    public List<RentViewModel> Rents;

    private readonly IRentApplication _rentApplication;
    private readonly IUserService _userService;

    public RentController(IRentApplication rentApplication, IUserService userService)
    {
        _rentApplication = rentApplication;
        _userService = userService;
    }
    public IActionResult Index()
    {
        return View();
    }
}
