using AutoMapper.Execution;
using LibBook.DomainContracts.Borrow;
using LibIdentity.Domain.UserAgg;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Versioning;
using Org.BouncyCastle.Utilities;
using X.PagedList;

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
    public async Task<IActionResult> EmployeeBorrows(int? page)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account", "adminPanel");
        }

        var loans = await _loanService.GetBorrowsByEmployeeId(user.Id.ToString());

        int pageNumber = page ?? 1;
        int pageSize = 6;
        var pagedLog = loans.ToPagedList(pageNumber, pageSize);

        return View("EmployeeBorrows", pagedLog);
    }
    [HttpGet]
    public async Task<IActionResult> MemberBorrows(string memberId, int? page)
    {
        var loans = await _loanService.GetBorrowsByMemberId(memberId);

        int pageNumber = page ?? 1;
        int pageSize = 6;
        var pagedLog = loans.ToPagedList(pageNumber, pageSize);

        return View("MemberBorrows", pagedLog);
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