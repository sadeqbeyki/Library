using Identity.Domain.Entities.User;
using Library.Application.CQRS.Queries.Lends;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Library.EndPoint.MVC.Areas.adminPanel.Controllers;

[Area("adminPanel")]
public class HomeController(UserManager<ApplicationUser> userManager,
    IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    private readonly UserManager<ApplicationUser> _userManager = userManager;

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

        var loans = await _mediator.Send(new GetLoansByEmployeeIdQuery(user.Id));

        int pageNumber = page ?? 1;
        int pageSize = 6;
        var pagedLog = loans.ToPagedList(pageNumber, pageSize);

        return View("EmployeeLoans", pagedLog);
    }
    [HttpGet]
    public async Task<IActionResult> MemberLoans(Guid memberId, int? page)
    {
        var loans = await _mediator.Send(new GetLoansByMemberIdQuery(memberId));

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
            var overdueLoans = await _mediator.Send(new GetOverdueLonesQuery());
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