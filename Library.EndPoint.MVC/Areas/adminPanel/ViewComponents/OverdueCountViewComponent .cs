using Library.Application.CQRS.Queries.Lends;
using Library.Application.DTOs.Lends;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.MVC.Areas.adminPanel.ViewComponents;
[ViewComponent(Name = "OverdueCount")]
public class OverdueCountViewComponent : ViewComponent
{

    private readonly IMediator _mediator;

    public OverdueCountViewComponent(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var overdueLoans = await _mediator.Send(new GetOverdueLonesQuery());
        return View(overdueLoans);
    }

    private async Task<List<LendDto>> GetItemsAsync()
    {
        return await _mediator.Send(new GetOverdueLonesQuery());
    }
}
