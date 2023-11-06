using Library.EndPoint.Areas.adminPanel.Models;
using LibBook.DomainContracts.Book;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LibInventory.DomainContracts.Inventory;
using Microsoft.AspNetCore.Authorization;

namespace Library.EndPoint.Areas.adminPanel.Controllers;
[Authorize(Roles ="admin, manager")]
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
        return RedirectToAction("Index", result);
        //return View("Index", result);
    }
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        EditInventory inventory = _inventoryService.GetDetails(id);
        inventory.Books = await _bookService.GetBooks();
        return View("Update", inventory);
    }
    [HttpPost]
    public async Task<IActionResult> Update(EditInventory command)
    {
        var result = await _inventoryService.Edit(command);
        return RedirectToAction("Index", result);
    }
    [HttpGet]
    public IActionResult Increase(int id)
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
    public ActionResult Decrease(int id)
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
    [HttpGet]
    public IActionResult OperationLog(int id)
    {
        var log = _inventoryService.GetOperationLog(id);
        return View("OperationLog", log);
    }
}
