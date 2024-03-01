using AppFramework.Infrastructure;
using Library.Application.Contracts;
using Library.Application.CQRS.Commands.Author;
using Library.Application.DTOs.Author;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.MVC.Areas.adminPanel.Controllers;
[Area("adminPanel")]
[Authorize(Roles = "Admin, Manager")]
public class AuthorsController : Controller
{
    private readonly IAuthorService _authorService;
    private readonly IMediator _mediator;

    public AuthorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    public async Task<ActionResult<List<AuthorDto>>> Index()
    {
        var result = await _authorService.GetAll();
        return View(result);
    }
    [HttpGet]
    public ActionResult<AuthorDto> Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<ActionResult> Create(AuthorDto authorDto)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var result = await _mediator.Send(new CreateAuthorCommand(authorDto));
        return RedirectToAction("Index", result);
    }
}
