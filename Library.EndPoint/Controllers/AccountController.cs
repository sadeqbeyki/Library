using LibIdentity.Domain.UserAgg;
using LibIdentity.DomainContracts.UserContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AppFramework.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace Library.EndPoint.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<LoginDto> _logger;

    public AccountController(UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<LoginDto> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var lastVisitedUrl = HttpContext.Session.GetString("LastVisitedUrl");
        HttpContext.Session.Remove("LastVisitedUrl");

        if (!string.IsNullOrEmpty(lastVisitedUrl))
        {
            return Redirect(lastVisitedUrl);
        }
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
        return Redirect(model?.ReturnUrl ?? "/");
        //return RedirectToAction("Index", model);

    }
    public IActionResult Login(string returnUrl, string errorMessage)
    {
        //return View(new LoginDto { ReturnUrl = returnUrl });

        if (!string.IsNullOrEmpty(errorMessage))
        {
            ModelState.AddModelError(string.Empty, errorMessage);
        }

        returnUrl ??= Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        return View(new LoginDto { ReturnUrl = returnUrl });
    }


    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto model)
    {
        //var returnUrl = model.ReturnUrl ??= Url.Content("~/");
        if (ModelState.IsValid)
        {
            User user = await _userManager.FindByNameAsync(model.Name);
            user ??= await _userManager.FindByEmailAsync(model.Name);

            if (user != null)
            {
                //await _signInManager.SignInAsync(user, isPersistent: false);
                var result = _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.IsCompletedSuccessfully)
                {
                    _logger.LogInformation("login log");
                    HttpContext.Session.SetString("LastVisitedUrl", model.ReturnUrl);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Redirect(model?.ReturnUrl ?? "/");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                return View(model);
            }
        }
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
