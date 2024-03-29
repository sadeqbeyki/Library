﻿using Library.Application.CQRS.Commands.Authors;
using Library.Application.CQRS.Commands.Books;
using Library.Application.CQRS.Queries.BookCategories;
using Library.Application.CQRS.Queries.Books;
using Library.Application.CQRS.Queries.Publishers;
using Library.Application.CQRS.Queries.Translators;
using Library.Application.DTOs.Books;
using Library.EndPoint.MVC.Areas.adminPanel.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.EndPoint.MVC.Areas.adminPanel.Controllers;

[Area("adminPanel")]
[Authorize(Roles = "Admin, Manager")]
public class BooksController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    #region Get
    [HttpGet]
    public async Task<IActionResult> Index(BookSearchModel searchModel)
    {
        BookWithCategoryViewModel model = new()
        {
            Books = await _mediator.Send(new SearchBookQuery(searchModel)),
            SearchModel = searchModel,
            Categories = new SelectList(await _mediator.Send(new GetBookCategoriesQuery()), "Id", "Name").ToList()
        };
        return View(model);
    }
    [HttpGet]
    public async Task<ActionResult<BookViewModel>> Details(int id)
    {
        var result = await _mediator.Send(new GetBookQuery(id));
        if (result == null)
            return NotFound();
        return View(result);
    }
    #endregion

    #region Create
    public async Task<ActionResult<BookDto>> Create()
    {
        var model = new BookCreateViewModel
        {
            BookCategories = await _mediator.Send(new GetBookCategoriesQuery()),
            Authors = (await _mediator.Send(new GetAuthorsQuery())).Select(author => author.Name).ToList(),
            Publishers = (await _mediator.Send(new GetPublishersQuery())).Select(publisher => publisher.Name).ToList(),
            Translators = (await _mediator.Send(new GetTranslatorsQuery())).Select(translator => translator.Name).ToList()
        };

        return View(model);
    }
    [HttpPost]
    public async Task<ActionResult> Create(CreateBookModel model)
    {
        var bookCategory = await _mediator.Send(new GetBookCategoryQuery(model.CategoryId));

        var selectedAuthors = Request.Form["selectedAuthors"].ToString();
        var selectedPublishers = Request.Form["selectedPublishers"].ToString();
        var selectedTranslators = Request.Form["selectedTranslators"].ToString();

        model.Category = bookCategory.Name;

        model.Authors = selectedAuthors.Split(',').ToList();
        model.Publishers = selectedPublishers.Split(',').ToList();
        model.Translators = selectedTranslators.Split(',').ToList();

        ModelState.Clear();
        TryValidateModel(model);
        if (ModelState.IsValid)
        {
            var result = await _mediator.Send(new CreateBookCommand(model));
            return RedirectToAction("Index", result);
        }
        return RedirectToAction("Create");
    }
    #endregion

    #region Update
    [HttpGet]
    public async Task<ActionResult<BookViewModel>> Update(int id)
    {
        var book = await _mediator.Send(new GetBookQuery(id));
        if (book != null)
        {
            UpdateBookViewModel model = new()
            {
                Book = book,
                BookCategories = await _mediator.Send(new GetBookCategoriesQuery()),
                Authors = (await _mediator.Send(new GetAuthorsQuery())).Select(author => author.Name).ToList(),
                Publishers = (await _mediator.Send(new GetPublishersQuery())).Select(publisher => publisher.Name).ToList(),
                Translators = (await _mediator.Send(new GetTranslatorsQuery())).Select(translator => translator.Name).ToList()
            };
            return View(model);
        }
        return NotFound();
    }
    [HttpPut, HttpPost]
    public async Task<IActionResult> Update(int id, UpdateBookViewModel model, IFormFile pictureFile)
    {
        var bookCategory = await _mediator.Send(new GetBookCategoryQuery(model.Book.CategoryId));

        var selectedAuthors = Request.Form["selectedAuthors"].ToString();
        var selectedPublishers = Request.Form["selectedPublishers"].ToString();
        var selectedTranslators = Request.Form["selectedTranslators"].ToString();

        model.Book.Category = bookCategory.Name;
        model.Book.Authors = [..selectedAuthors.Split(',')];
        model.Book.Publishers = [.. selectedPublishers.Split(',')];
        model.Book.Translators = [.. selectedTranslators.Split(',')];

        var result = await _mediator.Send(new UpdateBookCommand(model.Book, pictureFile));
        if (result.IsSucceeded)
            return RedirectToAction("Index");
        return View(model);
    }

    #endregion

    #region Delete
    [HttpGet]
    public async Task<ActionResult<BookViewModel>> Delete(int id)
    {
        var result = await _mediator.Send(new GetBookQuery(id));
        if (result == null)
            return NotFound();
        return View(result);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ConfirmDelete(int id)
    {
        await _mediator.Send(new RemoveBookCommand(id));
        return RedirectToAction("Index");
    }
    #endregion

}
