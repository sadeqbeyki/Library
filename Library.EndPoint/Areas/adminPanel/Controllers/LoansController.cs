using LMS.Contracts.Loan;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Areas.adminPanel.Controllers;
[Area("adminPanel")]
public class LoansController : Controller
{
    private readonly ILendService _loanService;

    public LoansController(ILendService loanService)
    {
        _loanService = loanService;
    }
   
    public async Task<ActionResult<List<LendDto>>> Index()
    {
        List<LendDto> loans = await _loanService.GetAllLends();
        return View("Index", loans);
    }
}
