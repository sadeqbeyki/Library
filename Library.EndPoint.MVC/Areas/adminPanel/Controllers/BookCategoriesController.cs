using Library.Application.CQRS.Commands.BookCategory;
using Library.Application.CQRS.Queries.BookCategory;
using Library.Application.DTOs.BookCategories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.MVC.Areas.adminPanel.Controllers;

[Area("adminPanel")]
[Authorize(Roles = "Admin, Manager")]
public class BookCategoriesController : Controller
{
    private readonly IMediator _mediator;

    public BookCategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ActionResult<List<BookCategoryDto>>> Index()
    {
        var result = await _mediator.Send(new GetBookCategoriesQuery());
        return View(result);
    }
    [HttpGet]
    public async Task<ActionResult<BookCategoryDto>> Details(int id)
    {
        var result = await _mediator.Send(new GetBookCategoryQuery(id));
        if (result == null)
            return NotFound();
        return View(result);
    }
    [HttpGet]
    public ActionResult<BookCategoryDto> Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<ActionResult> Create(BookCategoryDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var result = await _mediator.Send(new CreateBookCategoryCommand(dto));
        return RedirectToAction("Index", result);
    }
    [HttpGet]
    public async Task<ActionResult<BookCategoryDto>> Update(int id)
    {
        var category = await _mediator.Send(new GetBookCategoryQuery(id));
        if (category != null)
        {
            return View(category);
        }
        return View(category);
    }
    [HttpPost]
    public async Task<ActionResult> Update(int id, BookCategoryDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var result = await _mediator.Send(new UpdateBookCategoryCommand(id, dto));
        return RedirectToAction("Index", result);
    }
    [HttpGet]
    public async Task<ActionResult<BookCategoryDto>> Delete(int id)
    {
        var category = await _mediator.Send(new GetBookCategoryQuery(id));
        if (id == 0 || category == null)
        {
            return NoContent();
        }
        return View(category);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ConfirmDelete(int id)
    {
        await _mediator.Send(new DeleteBookCategoryCommand(id));
        return RedirectToAction("Index");
    }
}
