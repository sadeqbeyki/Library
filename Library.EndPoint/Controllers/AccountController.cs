using LibIdentity.Domain.UserAgg;
using LibIdentity.DomainContracts.UserContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AppFramework.Application;

namespace Library.EndPoint.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountController(UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Register(string returnUrl)
    {
        return View(new UserDto { ReturnUrl = returnUrl });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(UserDto model)
    {
        if (ModelState.IsValid)
        {
            User user = new()
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                BirthDate = model.BirthDate.ToGeorgianDateTime(),
            };
            var result = _userManager.CreateAsync(user, model.Password).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Login", result);
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
            }
        }
        return RedirectToAction("Index", model);
    }
    public IActionResult Login(string returnUrl)
    {
        return View(new LoginDto { ReturnUrl = returnUrl });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (ModelState.IsValid)
        {
            User user = await _userManager.FindByNameAsync(model.Name);
            user ??= await _userManager.FindByEmailAsync(model.Name);
            if (user != null)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                if ((await _signInManager.PasswordSignInAsync(user, model.Password, false, false)).Succeeded)
                {
                    return Redirect(model?.ReturnUrl ?? "/");
                }
            }
        }
        ModelState.AddModelError(" ", "Invalid name or password");
        return View(model);
    }
    public async Task<RedirectResult> Logout(string returnUrl = "/")
    {
        await _signInManager.SignOutAsync();
        return Redirect(returnUrl);
    }
    public IActionResult AccessDenied()
    {
        return View();
    }
}
