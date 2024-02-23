using AppFramework.Infrastructure;
using LibBook.DomainContracts.Translator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.MVC.Areas.adminPanel.Controllers;

[Area("adminPanel")]
[Authorize(Roles = "Admin, Manager")]
public class TranslatorsController : Controller
{
    private readonly ITranslatorService _translatorService;

    public TranslatorsController(ITranslatorService translatorService)
    {
        _translatorService = translatorService;
    }

    public async Task<ActionResult<List<TranslatorDto>>> Index()
    {
        var result = await _translatorService.GetAll();
        return View(result);
    }
    [HttpGet]
    public ActionResult<TranslatorDto> Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<ActionResult> Create(TranslatorDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var result = await _translatorService.Create(dto);
        return RedirectToAction("Index");
    }
}
