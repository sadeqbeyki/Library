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
    #endregion

    #region Create
    public async Task<ActionResult<BookDto>> Create()
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
    public async Task<ActionResult> Create(BookDto model)
    {
        var bookCategory = await _bookCategoryService.GetById(model.CategoryId);

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
        //return View(model);
        return RedirectToAction("Create");
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

    #region EditBook
    [HttpGet]
    public async Task<ActionResult> Edit(int id)
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

    [HttpPost]
    public async Task<ActionResult> Edit(int id, BookViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        var bookCategory = await _bookCategoryService.GetById(model.CategoryId);

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
            var result = await _bookService.Update(model); // اضافه کردن متد Update به سرویس کتاب
            if (result.IsSucceeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "خطا در ویرایش کتاب.");
            }
        }

        // در صورت بروز خطا، دوباره فرم ویرایش را نمایش دهید.
        var editModel = new EditBookViewModel
        {
            Book = model,
            BookCategories = await _bookCategoryService.GetCategories(),
            Authors = (await _authorService.GetAuthors()).Select(author => author.Name).ToList(),
            Publishers = (await _publisherService.GetPublishers()).Select(publisher => publisher.Name).ToList(),
            Translators = (await _translatorService.GetTranslators()).Select(translator => translator.Name).ToList()
        };
        return View(editModel);
    }
    #endregion


}
