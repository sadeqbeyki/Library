using AutoMapper;
using Identity.Application.DTOs.User;
using Identity.Application.Features.Command.User;
using Identity.Application.Features.Command.UserRole;
using Identity.Application.Features.Query.Role;
using Identity.Application.Features.Query.User;
using Identity.Application.Features.User.Queries;
using Library.EndPoint.MVC.Areas.adminPanel.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.MVC.Areas.adminPanel.Controllers;
[Area("adminPanel")]
[Authorize(Roles = "Admin")]

public class UsersController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UsersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<ActionResult> Index()
    {
        var users = await _mediator.Send(new GetAllUsersQuery());
        return View(users);
    }

    [HttpGet]
    public async Task<ActionResult> Details(Guid id)
    {
        var user = await _mediator.Send(new GetUserDetailsQuery(id));
        return View(user);
    }

    [HttpGet]
    public IActionResult Create(string returnUrl)
    {
        return View(new CreateUserDto { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserDto model)
    {
        var result = await _mediator.Send(new CreateUserCommand(model));
        return RedirectToAction("Index", result);
    }

    [HttpGet]
    public async Task<ActionResult> Update(Guid id)
    {
        var user = await _mediator.Send(new GetUserDetailsQuery(id));
        if (user == null)
        {
            return View();
        }
        var userMap = _mapper.Map<UpdateUserDto>(user);
        return View(userMap);
    }

    [HttpPost]
    public async Task<ActionResult> Update(UpdateUserDto dto)
    {
        var user = await _mediator.Send(new UpdateUserCommand(dto));
        return RedirectToAction("Index", user);
    }

    public async Task<ActionResult> Delete(Guid id)
    {
        var user = await _mediator.Send(new GetUserDetailsQuery(id));
        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (ModelState.IsValid)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));
            return RedirectToAction(nameof(Index));
        }
        return BadRequest();
    }

    public async Task<IActionResult> AssignRole()
    {
        var command = new UserRoleViewModel
        {
            Users = await _mediator.Send(new GetAllUsersQuery()),
            Roles = await _mediator.Send(new GetRolesQuery()),
            UserRoles = await _mediator.Send(new GetUserWithRolesQuery())
        };
        return View("AssignRole", command);
    }
    [HttpPost]
    public async Task<IActionResult> AssignRole(UserRoleViewModel model)
    {
        if (model.Assign.UserId == Guid.Empty || model.Assign.RoleId == Guid.Empty)
            return View(ViewBag.Error = "Fileds can't be null.");

        var result = await _mediator.Send(new AssignUserToRoleCommand(model.Assign));
        ViewBag.Success = result;
        return RedirectToAction("AssignRole");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveUserFromRole(Guid userId, Guid roleId)
    {
        var result = await _mediator.Send(new RemoveUserRoleCommand()
        {
            UserId = userId,
            RoleId = roleId
        });

        if (result)
            //ViewBag.Success = "Update user roles succeded!";
            return RedirectToAction("AssignRole");
        ViewBag.Error = "unsuccessfull!!";
        return RedirectToAction("AssignRole");
    }


}