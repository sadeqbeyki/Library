using Library.Application.CQRS.Commands.Authors;
using Library.Application.CQRS.Commands.Publishers;
using Library.Application.CQRS.Queries.Publishers;
using Library.Application.DTOs.Authors;
using Library.Application.DTOs.Publishers;
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
    public async Task<ActionResult> Details(int id)
    {
        var result = await _mediator.Send(new GetPublisherQuery(id));
        return View("Details", result);
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
    [HttpGet]
    public async Task<ActionResult<AuthorDto>> Update(int id)
    {
        var result = await _mediator.Send(new GetPublisherQuery(id));
        return View("Update", result);
    }
    [HttpPost]
    public async Task<ActionResult> Update(PublisherDto model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var result = await _mediator.Send(new UpdatePublisherCommand(model));
        return RedirectToAction("Index", result);
    }
    #region Delete
    [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new GetPublisherQuery(id));
        if (result == null)
            return NotFound();
        return View(result);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ConfirmDelete(int id)
    {
        await _mediator.Send(new DeletePublisherCommand(id));
        return RedirectToAction("Index");
    }
    #endregion
}
