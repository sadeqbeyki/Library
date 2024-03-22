using AppFramework.Application.Email;
using Identity.Application.Common.Exceptions;
using Identity.Application.DTOs.Auth;
using Identity.Application.DTOs.User;
using Identity.Application.Features.Command.Auth;
using Identity.Application.Features.Command.User;
using Identity.Application.Features.Query.Auth;
using Identity.Application.Features.Query.Email;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.MVC.Controllers;

public class AuthController : Controller
{
    private readonly IEmailService _emailService;
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator, IEmailService emailService)
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
            return RedirectToAction("Index", "Home", new { token = result.AccessToken });
        }
        return RedirectToAction("AccessDenied");
        //return response;

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
        var result = await _mediator.Send(new RegisterUserCommand(model));
        SendConfirmationLink(model.UserName, model.Email, result.confirmEmailToken);
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


    [HttpPost]
    public async Task<ActionResult<TokenAuthResponse>> Refresh(TokenAuthRequest model)
    {
        if (model is null)
            return BadRequest("Invalid client request");
        var result = await _mediator.Send(new RefreshTokenCommand(model));
        return result;
    }

    [HttpPost, Authorize]
    public IActionResult Revoke(string userName)
    {
        userName = User.Identity.Name;
        var result = _mediator.Send(new RevokeTokenCommand(userName));
        return View(result);
    }

    #region Confirm Mail
    private void SendConfirmationLink(string userName, string userMail, string confirmEmailToken)
    {
        //var scheme = HttpContext?.Request.Scheme ?? "https";
        var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { confirmEmailToken, email = userMail }, Request.Scheme);

        EmailModel message = new()
        {
            FromName = "Library Manager",
            FromAddress = "info@library.com",
            ToName = userName,
            ToAddress = userMail,
            Subject = "Confirm Your Registration",
            Content = "Please click the following link to confirm your registration: <a href=\"" + confirmationLink + "\">Confirm</a>"
        };
        _emailService.Send(message);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string token, string email)
    {
        if (token == null || email == null)
            throw new BadRequestException("Inputs cant be null!");
        var result = await _mediator.Send(new ConfirmEmailQuery(token, email));
        if (result.Succeeded)
            return View(nameof(ConfirmEmail));
        throw new BadRequestException("confirm link is expired!");
    }

    #endregion
}
