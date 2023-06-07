using LI.ApplicationContracts.UserContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Areas.adminPanel.Controllers;
[Area("adminPanel")]
public class UsersController : Controller
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ActionResult<List<UserDto>>> Index()
    {
        return await _userService.GetUsers();
    }

    [HttpGet]
    public async Task<ActionResult<UserDto>> GetUser(string id)
    {
        return await _userService.GetUser(id);
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser(UserDto model)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.CreateUser(model);
            return View(result);
        }
        return View(model);
    }

    [HttpGet]
    public async Task<ActionResult> UpdateUser(string id)
    {
        var user = await _userService.GetUser(id);
        if (user == null)
        {
            return View("Error");
        }
        return View(user);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(UserDto user)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.UpdateUser(user);
            return View(result);
        }
        return RedirectToAction("Index");
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUser(int id)
    {
        if (ModelState.IsValid)
        {
            await _userService.DeleteUser(id);
            return Ok();
        }
        return BadRequest();
    }
}