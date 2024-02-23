using Identity.Application.DTOs.Role;
using Identity.Application.Features.Command.Role;
using Identity.Application.Features.Query.Role;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.MVC.Areas.adminPanel.Controllers;
[Area("adminPanel")]
[Authorize(Roles = "Admin")]
public class RolesController : Controller
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var roles = await _mediator.Send(new GetRolesQuery());
        return View(roles);
    }
    public async Task<IActionResult> Details(Guid id)
    {
        await _mediator.Send(new GetRoleByIdQuery(id));
        return View();
    }
    #region Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(RoleDto dto)
    {
        var result = await _mediator.Send(new CreateRoleCommand(dto));
        return RedirectToAction("Index", result);
    }
    #endregion

    #region Update
    public async Task<ActionResult> Update(Guid id)
    {
        var role = await _mediator.Send(new GetRoleByIdQuery(id));
        if (role != null)
        {
            return View(role);
        }
        return RedirectToAction("Index");
    }
    [HttpPost]
    public async Task<ActionResult<RoleDto>> Update(RoleDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var result = await _mediator.Send(new UpdateRoleCommand(model));
        return RedirectToAction("Index", result);
    }
    #endregion

    #region Delete
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteRoleCommand(id));
        return RedirectToAction("Index", result);
    }
    #endregion
}
