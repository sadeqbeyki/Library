using Library.Application.CQRS.Commands.Authors;
using Library.Application.DTOs.Authors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.MVC.Areas.adminPanel.Controllers;
[Area("adminPanel")]
[Authorize(Roles = "Admin, Manager")]
public class AuthorsController : Controller
{
    private readonly IMediator _mediator;

    public AuthorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ActionResult<List<AuthorDto>>> Index()
    {
        var result = await _mediator.Send(new GetAuthorsQuery());
        return View(result);
    }
    [HttpGet]
    public async Task<ActionResult> Details(int id)
    {
        var result = await _mediator.Send(new GetAuthorByIdQuery(id));
        return View("Details", result);
    }
    [HttpGet]
    public ActionResult<AuthorDto> Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<ActionResult> Create(AuthorDto model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var result = await _mediator.Send(new CreateAuthorCommand(model));
        return RedirectToAction("Index", result);
    }
    [HttpGet]
    public async Task<ActionResult<AuthorDto>> Update([FromForm]int id)
    {
        var result = await _mediator.Send(new GetAuthorByIdQuery(id));
        return View("Update",result);
    }
    [HttpPost]
    public async Task<ActionResult> Update(int id, AuthorDto model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var result = await _mediator.Send(new UpdateAuthorCommand(id , model));
        return RedirectToAction("Index", result);
    }
}
