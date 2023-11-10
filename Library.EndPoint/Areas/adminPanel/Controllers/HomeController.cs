using AutoMapper.Execution;
using LibBook.DomainContracts.Borrow;
using LibIdentity.Domain.UserAgg;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Areas.adminPanel.Controllers;

[Area("adminPanel")]
public class HomeController : Controller
{
    private readonly ILoanService _borrowService;
    private readonly UserManager<UserIdentity> _userManager;
    public HomeController(ILoanService borrowService, UserManager<UserIdentity> userManager)
    {
        _borrowService = borrowService;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> EmployeeBorrows()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account","adminPanel");
        }

        var result = await _borrowService.GetBorrowsByEmployeeId(user.Id.ToString());
        return View("EmployeeBorrows", result);
    }
    [HttpGet]

    public async Task<IActionResult> MemberBorrows(string memberId)
    {
        var result = await _borrowService.GetBorrowsByMemberId(memberId);
        return View("MemberBorrows", result);
    }
    public IActionResult Profile()
    {
        return View();
    }
}