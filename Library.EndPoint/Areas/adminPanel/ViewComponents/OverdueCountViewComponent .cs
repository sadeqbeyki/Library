using LibBook.DomainContracts.Borrow;
using Library.EndPoint.Areas.adminPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.EndPoint.Areas.adminPanel.ViewComponents;
[ViewComponent(Name = "OverdueCount")]
public class OverdueCountViewComponent  : ViewComponent
{
    private readonly ILoanService _loanService;

    public OverdueCountViewComponent (ILoanService loanService)
    {
        _loanService = loanService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var overdueLoans = await _loanService.GetOverdueLones();
        return View(overdueLoans);
    }

    private async Task<List<LoanDto>> GetItemsAsync()
    {
        return await _loanService.GetOverdueLones();
    }
}
