using AppFramework.Infrastructure;
using AutoMapper;
using Identity.Application.DTOs.User;
using Identity.Application.Features.Command.User;
using Identity.Application.Features.Command.UserRole;
using Identity.Application.Features.Query.Role;
using Identity.Application.Features.Query.User;
using Identity.Application.Features.User.Queries;
using Library.EndPoint.Areas.adminPanel.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Areas.adminPanel.Controllers;
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
    public async Task<ActionResult> Details(string id)
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
    public async Task<ActionResult> Update(string id)
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
        //if (!ModelState.IsValid)
        //    return View(dto);

        var user = await _mediator.Send(new UpdateUserCommand(dto));
        return RedirectToAction("Index", user);
    }

    public async Task<ActionResult> Delete(string id)
    {
        var user = await _mediator.Send(new GetUserDetailsQuery(id));
        if (id == null || user == null)
        {
            return View();
        }
        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
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
        };
        return View("AssignRole", command);
    }
    [HttpPost]
    public async Task<IActionResult> AssignRole(UserRoleViewModel model)
    {
        var user = await _mediator.Send(new GetUserDetailsQuery(model.UserId));
        var role = await _mediator.Send(new GetRoleByIdQuery(model.RoleId));
        if (user == null || role == null)
        {
            ViewBag.CantBeNull = "Fileds can't be null.";
        }

        var isInRole = await _mediator.Send(new IsInRoleCommand(user.Id, role.Id));
        if (isInRole)
        {
            ViewBag.RoleExistError = "User is already in the selected role.";
        }
        else
        {
            var result = await _mediator.Send(new AssignUserToRoleCommand());
            if (result)
            {
                ViewBag.RoleAssigned = "Role assigned to User";
                return RedirectToAction("AssignRole");
            }
        }
        return RedirectToAction("AssignRole");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveUserFromRole(string userId, string roleId)
    {
        var user = await _mediator.Send(new GetUserDetailsQuery(userId));
        var role = await _mediator.Send(new GetRoleByIdQuery(roleId));

        if (user == null || role == null)
        {
            return BadRequest();
        }

        var result = await _mediator.Send(new UpdateUserRolesCommand());

        if (result == 1)
        {
            ViewBag.RoleAssigned = "Update user roles succeded!";
            //return RedirectToAction("AssignRole");
        }
        ViewBag.RoleExistError = "unsuccessfull!!";
        return RedirectToAction("AssignRole");
    }
}