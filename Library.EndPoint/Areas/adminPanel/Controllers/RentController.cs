using LI.ApplicationContracts.UserContracts;
using LMS.Contracts.Rent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static NuGet.Packaging.PackagingConstants;

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
    [HttpGet]
    public async Task<IActionResult> Index(RentSearchModel model)
    {
        Accounts = new SelectList(await _userService.GetUsers(), "Id", "FullName");
        Rents = _rentApplication.Search(model);
        return View();
    }
    [HttpGet]
    public IActionResult Cancel(Guid id)
    {
        _rentApplication.Cancel(id);
        return RedirectToPage("Index");
    }
    [HttpGet]
    public IActionResult OnGetItems(Guid id)
    {
        var items = _rentApplication.GetItems(id);
        return View("Items", items);
    }
}
