using Library.Application.Contracts;
using Library.Application.CQRS.Commands.Author;
using Library.Application.CQRS.Commands.Book;
using Library.Application.CQRS.Queries.BookCategory;
using Library.Application.DTOs.Book;
using Library.EndPoint.MVC.Areas.adminPanel.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.EndPoint.MVC.Areas.adminPanel.Controllers;

[Area("adminPanel")]
[Authorize(Roles = "Admin, Manager")]
public class BooksController : Controller
{
    public List<BookViewModel> Books = new();
    //public SelectList BookCategories;

    private readonly IBookService _bookService;
    private readonly IBookCategoryService _bookCategoryService;
    private readonly IAuthorService _authorService;
    private readonly IPublisherService _publisherService;
    private readonly ITranslatorService _translatorService;
    private readonly IMediator _mediator;



    public BooksController(IBookService bookService, IBookCategoryService bookCategoryService,
        IAuthorService authorService, IPublisherService publisherService, ITranslatorService translatorService, IMediator mediator)
    {
        _bookService = bookService;
        _bookCategoryService = bookCategoryService;
        _authorService = authorService;
        _publisherService = publisherService;
        _translatorService = translatorService;
        _mediator = mediator;
    }

    #region Get
    [HttpGet]
    public async Task<IActionResult> Index(BookSearchModel searchModel)
    {
        BookWithCategoryViewModel model = new()
        {
            Books = _bookService.Search(searchModel),
            SearchModel = searchModel,
            Categories = new SelectList(await _bookCategoryService.GetCategories(), "Id", "Name").ToList()
        };
        return View(model);
    }
    [HttpGet]
    public async Task<ActionResult<BookViewModel>> Details(int id)
    {
        var result = await _bookService.GetById(id);
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
        var book = await _bookService.GetById(id);
        if (book == null)
        {
            return NotFound();
        }

        var model = new EditBookViewModel
        {
            Book = book,
            BookCategories = await _bookCategoryService.GetCategories(),
            Authors = (await _authorService.GetAuthors()).Select(author => author.Name).ToList(),
            Publishers = (await _publisherService.GetPublishers()).Select(publisher => publisher.Name).ToList(),
            Translators = (await _translatorService.GetTranslators()).Select(translator => translator.Name).ToList()
        };

        return View(model);
    }

    [HttpPut, HttpPost]
    public async Task<IActionResult> Update(int id, EditBookViewModel model, IFormFile pictureFile)
    {
        if (id != model.Book.Id)
        {
            return BadRequest();
        }

        var bookCategory = await _bookCategoryService.GetById(model.Book.CategoryId);

        var selectedAuthors = Request.Form["selectedAuthors"].ToString();
        var selectedPublishers = Request.Form["selectedPublishers"].ToString();
        var selectedTranslators = Request.Form["selectedTranslators"].ToString();

        model.Book.Category = bookCategory.Name;
        model.Book.Authors = selectedAuthors.Split(',').ToList();
        model.Book.Publishers = selectedPublishers.Split(',').ToList();
        model.Book.Translators = selectedTranslators.Split(',').ToList();

        if (pictureFile != null && pictureFile.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                await pictureFile.CopyToAsync(memoryStream);
                model.Book.Picture = memoryStream.ToArray();
            }
        }
        //ModelState.Clear();
        //TryValidateModel(model);

        //if (!ModelState.IsValid)
        //{
        //    foreach (var modelState in ViewData.ModelState.Values)
        //    {
        //        foreach (var error in modelState.Errors)
        //        {
        //            return View(error);
        //        }
        //    }
        //}
        //else
        //{
        var result = await _bookService.Update(model.Book);
        if (result.IsSucceeded)
        {
            return RedirectToAction("Index");
        }

        else
        {
            ModelState.AddModelError(string.Empty, "خطا در ویرایش کتاب.");
        }
        //}

        return View(model);
    }
    #endregion

    #region Delete
    [HttpGet]
    public async Task<ActionResult<BookViewModel>> Delete(int id)
    {
        var result = await _bookService.GetById(id);
        if (result == null)
            return NotFound();
        return View(result);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ConfirmDelete(int id)
    {
        await _bookService.Delete(id);
        return RedirectToAction("Index");
    }
    #endregion

}
