using Library.Application.CQRS.Commands.Author;
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
}
