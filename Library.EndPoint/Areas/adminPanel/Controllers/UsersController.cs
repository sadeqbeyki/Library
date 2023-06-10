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

    public async Task<ActionResult<List<UpdateUserDto>>> Index()
    {
        var users = await _userService.GetUsers();
        return View(users);
    }

    [HttpGet]
    public async Task<ActionResult<UpdateUserDto>> Details(string id)
    {
        return await _userService.GetUser(id);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(UserDto model)
    {
        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                var errorMessage = error.ErrorMessage;
                var exception = error.Exception;
            }
        }
        var result = await _userService.CreateUser(model);
        return RedirectToAction("Index");
        //return View(model);
    }

    [HttpGet]
    public async Task<ActionResult> Update(string id)
    {
        var user = await _userService.GetUser(id);
        if (user == null)
        {
            return View("Error");
        }
        return View(user);
    }

    [HttpPut]
    public async Task<ActionResult<UpdateUserDto>> Update(UpdateUserDto user)
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