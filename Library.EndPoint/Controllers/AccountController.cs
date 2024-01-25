using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using AppFramework.Application.Email;
using Identity.Domain.Entities.User;
using Identity.Application.DTOs.User;
using MediatR;
using Identity.Application.Features.Command.User;
using Identity.Application.Features.Command.Auth;

namespace Library.EndPoint.Controllers;

public class AccountController : Controller
{

    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private readonly IMapper _mapper;
    private readonly IEmailService _email;
    private readonly IConfiguration _configuration;


    public AccountController(
        IMapper mapper,
        IEmailService email,
        IConfiguration configuration)
    {
        _mapper = mapper;
        _email = email;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        return View(new LoginUserDto { ReturnUrl = returnUrl });
    }
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginUserDto model)
    {
        IActionResult response = Unauthorized();
        model.ReturnUrl = model.ReturnUrl ?? Url.Content("~/");

        LoginCommand command = new(model);
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty,"modelstate invalid");
        }

        var result = await _mediator.Send(command);
        if (result != null)
        {
            return RedirectToAction("Index", "Home", new { token = result });
        }
        return response;
    }
    public IActionResult Register(string returnUrl)
    {
        return View(new CreateUserDto { ReturnUrl = returnUrl });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(CreateUserDto model)
    {
        if (!ModelState.IsValid)
        {
            return Redirect(model?.ReturnUrl ?? "/");
        }

        CreateUserCommand command = new(model);
        var result = await _mediator.Send(command);
        return RedirectToAction(nameof(SuccessRegistration));
    }
    [HttpGet]
    //public async Task<IActionResult> ConfirmEmail(string token, string email)
    //{
    //    var user = await _userManager.FindByEmailAsync(email);
    //    if (user == null)
    //        return View("Error");

    //    var result = await _userManager.ConfirmEmailAsync(user, token);
    //    return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
    //}
    [HttpGet]
    public IActionResult SuccessRegistration()
    {
        return View();
    }
    public IActionResult Error()
    {
        return View();
    }
    //public async Task<RedirectResult> Logout(string returnUrl = "/")
    //{
    //    await _signInManager.SignOutAsync();
    //    return Redirect(returnUrl);
    //}
    public IActionResult AccessDenied()
    {
        return View();
    }
}
