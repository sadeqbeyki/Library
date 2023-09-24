using AppFramework.Infrastructure;
using LibBook.Domain.AuthorAgg;
using LibBook.DomainContracts.Author;
using LibBook.DomainContracts.Book;
using LibBook.DomainContracts.BookCategory;
using LibBook.DomainContracts.Publisher;
using LibBook.DomainContracts.Translator;
using Library.EndPoint.Areas.adminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Library.EndPoint.Areas.adminPanel.Controllers;

[Area("adminPanel")]
[Authorize(Roles = "admin, manager")]
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
    #region Get
    [HttpGet]
    public async Task<ActionResult<List<BookViewModel>>> Index()
    {
        var result = await _bookService.GetAllBooks();
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
    #endregion

    #region Create
    public async Task<ActionResult<CreateBookDto>> Create()
    {
        var model = new BookCreateViewModel
        {
            BookCategories = await _bookCategoryService.GetCategories(),
            Authors = (await _authorService.GetAuthors()).Select(author => author.Name).ToList(),
            Publishers = (await _publisherService.GetPublishers()).Select(publisher => publisher.Name).ToList(),
            Translators = (await _translatorService.GetTranslators()).Select(translator => translator.Name).ToList()
        };

        return View(model);
    }
    [HttpPost]
    public async Task<ActionResult> Create(CreateBookDto model)
    {
        // بازیابی مقادیر ارسالی از Request.Form
        var selectedAuthors = Request.Form["SelectedAuthors"].ToString();
        var selectedPublishers = Request.Form["SelectedPublishers"].ToString();
        var selectedTranslators = Request.Form["SelectedTranslators"].ToString();

        // اکنون شما می‌توانید از این مقادیر در ادامه اکشن استفاده کنید.

        // تبدیل مقادیر رشته‌ای به لیست
        //model.CategoryId = CategoryId;
        model.Authors = selectedAuthors.Split(',').ToList();
        model.Publishers = selectedPublishers.Split(',').ToList();
        model.Translators = selectedTranslators.Split(',').ToList();

        if (ModelState.IsValid)
        {
            var result = await _bookService.Create(model);
            if (result.IsSucceeded)
            {
                return RedirectToAction("Index", result);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "خطا در ایجاد کتاب.");
            }
        }
        return View(model);
    }
    #endregion

    #region Update
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
