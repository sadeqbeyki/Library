using LibIdentity.Domain.UserAgg;
using LibIdentity.DomainContracts.UserContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace Library.EndPoint.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<LoginDto> _logger;
    private readonly IMapper _mapper;
    //private readonly EmailSender _emailSender;

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
        if (!ModelState.IsValid)
        {
            return Redirect(model?.ReturnUrl ?? "/");

        }

        var email = await _userManager.FindByEmailAsync(model.Email);
        if (email != null)
        {
            ModelState.AddModelError("Email", "این ایمیل قبلا ثبت شده است");
            return View(model);
        }

        var user = _mapper.Map<User>(model);
        var result = _userManager.CreateAsync(user, model.Password).Result;
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return View(model);
        }


        await _userManager.AddToRoleAsync(user, "member");

        return RedirectToAction("Login", result);
        //return RedirectToAction(nameof(SuccessRegistration));

    }
    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return View("Error");
        var result = await _userManager.ConfirmEmailAsync(user, token);
        return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
    }
    [HttpGet]
    public IActionResult SuccessRegistration()
    {
        return View();
    }
    public IActionResult Login(string returnUrl)
    {
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
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return Redirect(model?.ReturnUrl ?? "/");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                return View();
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
