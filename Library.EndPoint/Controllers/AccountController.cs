using LibIdentity.Domain.UserAgg;
using LibIdentity.DomainContracts.UserContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using AppFramework.Application.Email;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibIdentity.ApplicationServices;

namespace Library.EndPoint.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly SignInManager<UserIdentity> _signInManager;
    private readonly ILogger<LoginViewModel> _logger;
    private readonly IMapper _mapper;
    private readonly IEmailService _email;
    private readonly IConfiguration _configuration;

    public AccountController(UserManager<UserIdentity> userManager,
        SignInManager<UserIdentity> signInManager,
        ILogger<LoginViewModel> logger,
        IMapper mapper,
        IEmailService email,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _mapper = mapper;
        _email = email;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return View();
    }


    public IActionResult Register(string returnUrl)
    {
        return View(new CreateUserViewModel { ReturnUrl = returnUrl });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(CreateUserViewModel model)
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

        //string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
        var user = _mapper.Map<UserIdentity>(model);
        var result = _userManager.CreateAsync(user, model.Password).Result;
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return View(model);
        }

        //confirmation email 

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);

        EmailModel message = new()
        {
            FromName = "Library Manager",
            FromAddress = "info@library.com",
            ToName = user.UserName,
            ToAddress = user.Email,
            Subject = "Confirm Your Registration",
            Content = "Please click the following link to confirm your registration: <a href=\"" + confirmationLink + "\">Confirm</a>"
        };
        _email.Send(message);

        //add to default role
        await _userManager.AddToRoleAsync(user, "member");

        // Generate JWT token
        var jwtService = new JwtService();
        var tokenNew = jwtService.GenerateJwtToken(user);

        //return RedirectToAction("Login", result);
        return RedirectToAction(nameof(SuccessRegistration));
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


    public IActionResult Error()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        model.ReturnUrl = model.ReturnUrl ?? Url.Content("~/");
        if (ModelState.IsValid)
        {
            UserIdentity user = await _userManager.FindByNameAsync(model.Name) ?? await _userManager.FindByEmailAsync(model.Name);

            //if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            //{
            //    return BadRequest("wrong password!");
            //}
            //string token = CreateToken(user);
            //return View(token);
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

    private string CreateToken(UserIdentity user)
    {
        List<Claim> claims = new() { new Claim(ClaimTypes.Name, user.UserName) };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

}
