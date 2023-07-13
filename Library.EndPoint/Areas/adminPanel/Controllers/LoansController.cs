using LMS.Contracts.Loan;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Areas.adminPanel.Controllers;
[Area("adminPanel")]
public class LoansController : Controller
{
    private readonly ILoanService _loanService;

    public LoansController(ILoanService loanService)
    {
        _loanService = loanService;
    }
   
    public async Task<ActionResult<List<LoanDto>>> Index()
    {
        List<LoanDto> loans = await _loanService.GetAllLoans();
        return View("Index", loans);
    }
}
