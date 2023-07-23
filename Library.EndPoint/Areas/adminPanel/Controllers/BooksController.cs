using LibBook.DomainContracts.Author;
using LibBook.DomainContracts.Book;
using LibBook.DomainContracts.BookCategory;
using LibBook.DomainContracts.Publisher;
using LibBook.DomainContracts.Translator;
using Library.EndPoint.Areas.adminPanel.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Areas.adminPanel.Controllers;

[Area("adminPanel")]
public class BooksController : Controller
{
    private readonly IBookService _bookService;
    private readonly IBookCategoryService _bookCategoryService;
    private readonly IAuthorService _authorService;
    private readonly IPublisherService _publisherService;
    private readonly ITranslatorService _translatorService;

    public BooksController(IBookService bookService, IBookCategoryService bookCategoryService,
        IAuthorService authorService, IPublisherService publisherService, ITranslatorService translatorService)
    {
        _bookService = bookService;
        _bookCategoryService = bookCategoryService;
        _authorService = authorService;
        _publisherService = publisherService;
        _translatorService = translatorService;
    }
    [HttpGet]
    public async Task<ActionResult<List<BookViewModel>>> Index()
    {
        var result = await _bookService.GetAll();
        return View(result);
    }
    [HttpGet]
    public async Task<ActionResult<BookViewModel>> Details(int id)
    {
        var result = await _bookService.GetById(id);
        if (result == null)
            return NotFound();
        return View(result);
    }
    [HttpGet]
    public async Task<ActionResult<BookDto>> Create()
    {
        var command = new CreateBookViewModel
        {
            BookCategories = await _bookCategoryService.GetCategories(),
            Authors = await _authorService.GetAuthors(),
            Publishers = await _publisherService.GetPublishers(),
            Translators = await _translatorService.GetTranslators()
        };
        return View("Create", command);
    }
    [HttpPost]
    public async Task<ActionResult> Create(BookDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var result = await _bookService.Create(dto);
        return RedirectToAction("Index", result);
    }
    [HttpGet]
    public async Task<ActionResult<BookViewModel>> Update(int id)
    {
        var model = new UpdateBookViewModel
        {
            Book = await _bookService.GetById(id),
            BookCategories = await _bookCategoryService.GetCategories(),
            Authors = await _authorService.GetAuthors(),
            Publishers = await _publisherService.GetPublishers(),
            Translators = await _translatorService.GetTranslators()
        };

        return View("Update", model);
    }
    [HttpPut, HttpPost]
    public async Task<ActionResult> Update(UpdateBookViewModel model)
    {
        var result = await _bookService.Update(model.Book);
        return RedirectToAction("Index", result);
    }
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
}
