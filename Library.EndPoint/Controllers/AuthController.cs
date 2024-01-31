using AppFramework.Application.Email;
using Identity.Application.DTOs.Auth;
using Identity.Application.DTOs.User;
using Identity.Application.Features.Command.Auth;
using Identity.Application.Features.Command.User;
using Identity.Application.Features.Query.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Controllers;

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


    [HttpPost]
    public async Task<ActionResult<AuthenticatedResponse>> Refresh(TokenDto model)
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

    #region extention method
    //private bool SendConfirmationLink(CreateUserDto user)
    //{
    //    var emailConfirmToken = _mediator.Send(new GetConfirmEmailTokenQuery(user.));

    //    var scheme = HttpContext?.Request.Scheme ?? "https";
    //    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { emailConfirmToken, email = user.Email }, scheme);

    //    EmailModel message = new()
    //    {
    //        FromName = "Library Manager",
    //        FromAddress = "info@library.com",
    //        ToName = user.UserName,
    //        ToAddress = user.Email,
    //        Subject = "Confirm Your Registration",
    //        Content = "Please click the following link to confirm your registration: <a href=\"" + confirmationLink + "\">Confirm</a>"
    //    };
    //    _emailService.Send(message);
    //    return true;
    //}
    //[HttpGet]
    //public async Task<IActionResult> ConfirmEmail(string token, string email)
    //{
    //    var user = await _userManager.FindByEmailAsync(email);
    //    if (user == null)
    //        return View("Error");

    //    var result = await _userManager.ConfirmEmailAsync(user, token);
    //    return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
    //}

    #endregion
}
