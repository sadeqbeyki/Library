using AutoMapper.Execution;
using LibBook.DomainContracts.Borrow;
using LibIdentity.Domain.UserAgg;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Versioning;

namespace Library.EndPoint.Areas.adminPanel.Controllers;

[Area("adminPanel")]
public class HomeController : Controller
{
    private readonly ILoanService _loanService;
    private readonly UserManager<UserIdentity> _userManager;

    private readonly SignInManager<UserIdentity> _signInManager;
    public HomeController(ILoanService loanService,
        UserManager<UserIdentity> userManager)
    {
        _loanService = loanService;
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
            return RedirectToAction("Login", "Account", "adminPanel");
        }

        var result = await _loanService.GetBorrowsByEmployeeId(user.Id.ToString());
        return View("EmployeeBorrows", result);
    }
    [HttpGet]
    public async Task<IActionResult> MemberBorrows(string memberId)
    {
        var result = await _loanService.GetBorrowsByMemberId(memberId);
        return View("MemberBorrows", result);
    }


    public async Task<int> GetOverdueLoansCount()
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = User;
            var overdueLoans = await _loanService.GetOverdueLones();
            return overdueLoans.Count;
        }
        else
        {
            return 0;
        }
    }
    public IActionResult Profile()
    {
        return View();
    }
}