using LibBook.DomainContracts.Author;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Areas.adminPanel.Controllers;
[Area("adminPanel")]
public class AuthorsController : Controller
{
    private readonly IAuthorService _authorService;

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
        var result = await _authorService.Create(authorDto);
        return RedirectToAction("Index");
    }
}
