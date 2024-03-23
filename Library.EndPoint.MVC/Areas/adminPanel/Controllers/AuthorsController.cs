using Library.Application.CQRS.Commands.Authors;
using Library.Application.CQRS.Commands.Books;
using Library.Application.CQRS.Queries.Books;
using Library.Application.DTOs.Authors;
using Library.Application.DTOs.Books;
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
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("AccessDenied", "Error");
        }
        var result = await _mediator.Send(new GetAuthorsQuery());
        return View(result);
        //return RedirectToAction("AccessDenied");

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
    public async Task<ActionResult<AuthorDto>> Update(int id)
    {
        var result = await _mediator.Send(new GetAuthorByIdQuery(id));
        return View("Update",result);
    }
    [HttpPost]
    public async Task<ActionResult> Update(AuthorDto model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var result = await _mediator.Send(new UpdateAuthorCommand(model));
        return RedirectToAction("Index", result);
    }
    #region Delete
    [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new GetAuthorByIdQuery(id));
        if (result == null)
            return NotFound();
        return View(result);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ConfirmDelete(int id)
    {
        await _mediator.Send(new RemoveAuthorCommand(id));
        return RedirectToAction("Index");
    }
    #endregion
}
