using Library.Application.CQRS.Commands.Author;
using Library.Application.CQRS.Commands.Publisher;
using Library.Application.DTOs.Publisher;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.MVC.Areas.adminPanel.Controllers;
[Area("adminPanel")]
[Authorize(Roles = "Admin, Manager")]
public class PublishersController : Controller
{
    private readonly IMediator _mediator;

    public PublishersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ActionResult<List<PublisherDto>>> Index()
    {
        var result = await _mediator.Send(new GetPublishersQuery());
        return View(result);
    }
    [HttpGet]
    public ActionResult<PublisherDto> Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<ActionResult> Create(PublisherDto model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var result = await _mediator.Send(new CreatePublisherCommand(model));
        return RedirectToAction("Index");
    }
}
