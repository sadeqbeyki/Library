using LibIdentity.Domain.UserAgg;
using LibIdentity.DomainContracts.UserContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using AutoMapper;

namespace Library.EndPoint.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<LoginDto> _logger;
    private readonly IMapper _mapper;

    public AccountController(UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<LoginDto> logger,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _mapper = mapper;
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
    public async Task<IActionResult> Register(UserDto model)
    {
        if (ModelState.IsValid)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "این ایمیل قبلا ثبت شده است");
                return View(model);
            }

            var userMap = _mapper.Map<User>(model);
            var result = _userManager.CreateAsync(userMap, model.Password).Result;
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(userMap, "member");
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
    }
    public IActionResult Login(string returnUrl)
    {
        //returnUrl ??= Url.Content("~/");

        return View(new LoginDto { ReturnUrl = returnUrl });
    }


    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (ModelState.IsValid)
        {
            User user = await _userManager.FindByNameAsync(model.Name);
            user ??= await _userManager.FindByEmailAsync(model.Name);

            if (user != null)
            {
                var result =await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("login log");
                    HttpContext.Session.SetString("LastVisitedUrl", model.ReturnUrl);
                    return Redirect(model?.ReturnUrl ?? "/");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
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
