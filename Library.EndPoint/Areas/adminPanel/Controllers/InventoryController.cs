using BI.ApplicationContracts.Inventory;
using Library.EndPoint.Areas.adminPanel.Models;
using LMS.Contracts.Book;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.EndPoint.Areas.adminPanel.Controllers;
[Area("adminPanel")]
public class InventoryController : Controller
{
    public List<InventoryViewModel> Inventory;
    public List<SelectListItem> Books { get; set; }
    public InventorySearchModel SearchModel { get; set; }


    private readonly IBookService _bookService;
    private readonly IInventoryService _inventoryService;

    public InventoryController(IBookService bookService, IInventoryService inventoryService)
    {
        _bookService = bookService;
        _inventoryService = inventoryService;
    }

    public async Task<IActionResult> Index(InventorySearchModel searchModel)
    {
        Inventory = _inventoryService.Search(searchModel);

        var model = new InventoryViewModelWithSearchModel
        {
            Inventory = Inventory,
            SearchModel = searchModel,
            Books = new SelectList(await _bookService.GetBooks(), "Id", "Title").ToList(),
        };

        return View(model);
    }

    public async Task<ActionResult> Create()
    {
        var command = new CreateInventory
        {
            Books = await _bookService.GetBooks()
        };
        return View("Create", command);
    }
    [HttpPost]
    public async Task<ActionResult> Create(CreateInventory command)
    {
        var result = await _inventoryService.Create(command);
        return View("Index", result);
    }
    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        EditInventory inventory = _inventoryService.GetDetails(id);
        inventory.Books = await _bookService.GetBooks();
        return View("Update", inventory);
    }
    [HttpPost]
    public async Task<IActionResult> Update(EditInventory command)
    {
        var result = await _inventoryService.Edit(command);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult Increase(Guid id)
    {
        var command = new IncreaseInventory()
        {
            InventoryId = id
        };
        return View("Increase", command);
    }
    [HttpPost]
    public async Task<IActionResult> Increase(IncreaseInventory command)
    {
        var result = await _inventoryService.Increase(command);
        return RedirectToAction("Index", result);
    }
    [HttpGet]
    public ActionResult Decrease(Guid id)
    {
        var command = new DecreaseInventory()
        {
            InventoryId = id
        };
        return View("Decrease", command);
    }
    [HttpPost]
    public async Task<IActionResult> Decrease(DecreaseInventory command)
    {
        var result = await _inventoryService.Decrease(command);
        return RedirectToAction("Index", result);
    }
}
