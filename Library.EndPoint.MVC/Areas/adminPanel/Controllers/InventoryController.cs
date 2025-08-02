using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;
using Library.EndPoint.MVC.Helper;
using Library.EndPoint.MVC.Areas.adminPanel.Models;
using Warehouse.Application.DTOs;
using MediatR;
using Warehouse.Application.DTOs.InventoryOperation;
using Warehouse.Application.DTOs.Inventories;
using Warehouse.Application.CQRS.Queries.Inventories;
using Warehouse.Application.CQRS.Commands.Inventories;
using Library.Application.CQRS.Queries.Books;
using X.PagedList.Extensions;

namespace Library.EndPoint.MVC.Areas.adminPanel.Controllers;
[Authorize(Roles = "Admin, Manager")]
[Area("adminPanel")]
public class InventoryController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    public async Task<IActionResult> Index(InventorySearchModel searchModel, int? page)
    {
        const int pageSize = 4;
        var result = await _mediator.Send(new SearchInventoryQuery(searchModel));
        var paginatedInventory = PaginatedList<InventoryViewModel>.Create(result, page ?? 1, pageSize);
        var model = new InventoryViewModelWithSearchModel
        {
            Inventory = paginatedInventory,
            SearchModel = searchModel,
            Books = new SelectList(await _mediator.Send(new GetBooksQuery()), "Id", "Title").ToList(),
        };

        return View(model);
    }

    public async Task<ActionResult> Create()
    {
        var command = new CreateInventory
        {
            Books = await _mediator.Send(new GetBooksQuery())
        };
        return View("Create", command);
    }
    [HttpPost]
    public async Task<ActionResult> Create(CreateInventory model)
    {
        var result = await _mediator.Send(new CreateInventoryCommand(model));
        return RedirectToAction("Index", result);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var result = await _mediator.Send(new GetInventoryQuery(id));
        result.Books = await _mediator.Send(new GetBooksQuery());
        return View("Update", result);
    }
    [HttpPost]
    public async Task<IActionResult> Update(EditInventory command)
    {
        var result = await _mediator.Send(new UpdateInventoryCommand(command));
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
        var result = await _mediator.Send(new IncreaseInventoryItemCommand(command));
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
        var result = await _mediator.Send(new DecreaseInventoryItemCommand(command));
        return RedirectToAction("Index", result);
    }
    [HttpGet]
    public async Task<ActionResult>OperationLog(int id, int? page)
    {
        var log = await _mediator.Send(new GetOperationLogQuery(id));

        int pageSize = 6;
        var pagedLog = log.ToPagedList(page ?? 1, pageSize);

        return View("OperationLog", pagedLog);
    }
}
