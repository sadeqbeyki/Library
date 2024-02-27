using Library.Application.Contracts;
using Library.Application.DTOs.Lend;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.MVC.Areas.adminPanel.ViewComponents;
[ViewComponent(Name = "OverdueCount")]
public class OverdueCountViewComponent : ViewComponent
{
    private readonly ILendService _loanService;

    public OverdueCountViewComponent(ILendService loanService)
    {
        _loanService = loanService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var overdueLoans = await _loanService.GetOverdueLones();
        return View(overdueLoans);
    }

    private async Task<List<LendDto>> GetItemsAsync()
    {
        return await _loanService.GetOverdueLones();
    }
}
