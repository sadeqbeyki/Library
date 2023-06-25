using LMS.Contracts.Author;
using LMS.Contracts.Translator;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Areas.adminPanel.Controllers;

[Area("adminPanel")]
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
