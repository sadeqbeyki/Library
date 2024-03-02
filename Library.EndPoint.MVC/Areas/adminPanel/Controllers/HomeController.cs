using Identity.Domain.Entities.User;
using Library.Application.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Library.EndPoint.MVC.Areas.adminPanel.Controllers;

[Area("adminPanel")]
public class HomeController : Controller
{
    private readonly ILendService _loanService;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILendService loanService,
        UserManager<ApplicationUser> userManager)
    {
        _loanService = loanService;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> EmployeeLoans(int? page)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Auth", "adminPanel");
        }

        var loans = await _loanService.GetLoansByEmployeeId(user.Id);

        int pageNumber = page ?? 1;
        int pageSize = 6;
        var pagedLog = loans.ToPagedList(pageNumber, pageSize);

        return View("EmployeeLoans", pagedLog);
    }
    [HttpGet]
    public async Task<IActionResult> MemberLoans(Guid memberId, int? page)
    {
        var loans = await _loanService.GetLoansByMemberId(memberId);

        int pageNumber = page ?? 1;
        int pageSize = 6;
        var pagedLog = loans.ToPagedList(pageNumber, pageSize);

        return View("MemberLoans", pagedLog);
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