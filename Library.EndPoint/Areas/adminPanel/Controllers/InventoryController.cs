using BI.ApplicationContracts.Inventory;
using Library.EndPoint.Areas.adminPanel.Models;
using LMS.Contracts.Book;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.EndPoint.Areas.adminPanel.Controllers;
[Area("adminPanel")]
public class InventoryController : Controller
{
    //public SelectList books;
    public List<InventoryViewModel> Inventory;
    public InventorySearchModel SearchModel { get; set; }


    private readonly IBookService _bookService;
    private readonly IInventoryService _inventoryService;

    public InventoryController(IBookService bookService, IInventoryService inventoryService)
    {
        _bookService = bookService;
        _inventoryService = inventoryService;
    }

    //public async Task<IActionResult> Index(InventorySearchModel searchModel)
    //{
    //    Books = new SelectList(await _bookService.GetBooks(), "Id", "Title");
    //    Inventory = _inventoryService.Search(searchModel);

    //    return View(Inventory);
    //}

    public async Task<IActionResult> Index(InventorySearchModel searchModel)
    {
        //books = new SelectList(await _bookService.GetBooks(), "Id", "Title");
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
        return View(result);
    }
    //public async Task<IActionResult> Index()
    //{
    //    List<BookViewModel> books = await _bookService.GetBooks();
    //    return View(books);
    //}
}
