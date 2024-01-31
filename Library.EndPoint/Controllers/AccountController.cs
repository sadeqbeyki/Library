using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Identity.Application.DTOs.User;
using MediatR;
using Identity.Application.Features.Command.User;
using Identity.Application.Features.Command.Auth;
using Identity.Application.Features.Query.Auth;
using AppFramework.Application.Email;

namespace Library.EndPoint.Controllers;

public class AccountController : Controller
{

    private readonly IMediator _mediator;
    private readonly IEmailService _emailService;
    public AccountController(IMediator mediator, IEmailService emailService)
    {
        _mediator = mediator;
        _emailService = emailService;
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
        model.ReturnUrl ??= Url.Content("~/");

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "modelstate invalid");
        }

        var result = await _mediator.Send(new AuthCommand(model));
        if (result != null)
        {
            return RedirectToAction("Index", "Home", new { token = result });
        }
        return response;

    }
    [HttpGet]
    public IActionResult Register(string returnUrl)
    {
        return View(new CreateUserDto { ReturnUrl = returnUrl });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(CreateUserDto model)
    {
        //if (!ModelState.IsValid)
        //{
        //    return Redirect(model?.ReturnUrl ?? "/");
        //}

        var result = await _mediator.Send(new RegisterUserCommand(model));
        //SendConfirmationLink();
        return RedirectToAction(nameof(SuccessRegistration));
    }

    [HttpGet]
    public IActionResult SuccessRegistration()
    {
        return View();
    }
    public IActionResult Error()
    {
        return View();
    }
    public async Task<RedirectResult> Logout(string returnUrl = "/")
    {
        await _mediator.Send(new LogOutQuery(returnUrl));
        return Redirect(returnUrl);
    }
    public IActionResult AccessDenied()
    {
        return View();
    }

 
}
