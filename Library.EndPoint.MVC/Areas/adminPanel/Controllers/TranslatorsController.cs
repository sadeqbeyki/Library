using Library.Application.CQRS.Commands.Authors;
using Library.Application.CQRS.Commands.Translators;
using Library.Application.CQRS.Queries.Translators;
using Library.Application.DTOs.Authors;
using Library.Application.DTOs.Translators;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.MVC.Areas.adminPanel.Controllers;

[Area("adminPanel")]
[Authorize(Roles = "Admin, Manager")]
public class TranslatorsController : Controller
{
    private readonly IMediator _mediator;

    public TranslatorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ActionResult<List<TranslatorDto>>> Index()
    {
        var result = await _mediator.Send(new GetTranslatorsQuery());
        return View(result);
    }
    [HttpGet]
    public ActionResult<TranslatorDto> Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<ActionResult> Create(TranslatorDto model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var result = await _mediator.Send(new CreateTranslatorCommand(model));
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<ActionResult<TranslatorDto>> Update([FromForm] int id)
    {
        var result = await _mediator.Send(new GetTranslatorQuery(id));
        return View("Update", result);
    }
    [HttpPost]
    public async Task<ActionResult> Update(TranslatorDto model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var result = await _mediator.Send(new UpdateTranslatorCommand(model));
        return RedirectToAction("Index", result);
    }
}
