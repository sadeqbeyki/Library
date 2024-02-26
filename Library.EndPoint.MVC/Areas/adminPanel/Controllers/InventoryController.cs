using LibBook.DomainContracts.Book;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;
using Library.EndPoint.MVC.Helper;
using Library.EndPoint.MVC.Areas.adminPanel.Models;
using Warehouse.Application.DTOs;
using Warehouse.Service.Contracts;
using Warehouse.Application.DTOs.Inventory;
using Warehouse.Application.CQRS.Commands.Inventory;
using MediatR;
using Warehouse.Application.CQRS.Queries.Inventory;

namespace Library.EndPoint.MVC.Areas.adminPanel.Controllers;
[Authorize(Roles = "Admin, Manager")]
[Area("adminPanel")]
public class InventoryController : Controller
{
    private readonly IBookService _bookService;
    private readonly IInventoryService _inventoryService;
    private readonly IMediator _mediator;

    public InventoryController(IBookService bookService, IInventoryService inventoryService, IMediator mediator)
    {
        _bookService = bookService;
        _inventoryService = inventoryService;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(InventorySearchModel searchModel, int? page)
    {
        const int pageSize = 4;
        var result = await _mediator.Send(new SearchInventoryQuery(searchModel));
        var paginatedInventory = PaginatedList<InventoryViewModel>.Create(result, page ?? 1, pageSize);
        var model = new InventoryViewModelWithSearchModel
        {
            Inventory = paginatedInventory,
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
    public async Task<ActionResult> Create(CreateInventoryCommand command)
    {
        var result = await _mediator.Send(command);
        return RedirectToAction("Index", result);
    }

    //[HttpPost]
    //public async Task<ActionResult> Create(CreateInventory command)
    //{
    //    var result = await _inventoryService.Create(command);
    //    return RedirectToAction("Index", result);
    //}

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
    public IActionResult OperationLog(int id, int? page)
    {
        var log = _inventoryService.GetOperationLog(id);

        int pageSize = 6;
        var pagedLog = log.ToPagedList(page ?? 1, pageSize);

        return View("OperationLog", pagedLog);
    }
}
