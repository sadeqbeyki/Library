using LibBook.DomainContracts.Borrow;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.MVC.Areas.adminPanel.ViewComponents;
[ViewComponent(Name = "OverdueCount")]
public class OverdueCountViewComponent : ViewComponent
{
    private readonly ILoanService _loanService;

    public OverdueCountViewComponent(ILoanService loanService)
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
