using LMS.Contracts.Publisher;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Areas.adminPanel.Controllers;
[Area("adminPanel")]
public class PublishersController : Controller
{
    private readonly IPublisherService _publisherService;

    public PublishersController(IPublisherService publisherService)
    {
        _publisherService = publisherService;
    }

    public async Task<ActionResult<List<PublisherDto>>> Index()
    {
        var result = await _publisherService.GetAll();
        return View(result);
    }
    [HttpGet]
    public ActionResult<PublisherDto> Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<ActionResult> Create(PublisherDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var result = await _publisherService.Create(dto);
        return RedirectToAction("Index");
    }
}
