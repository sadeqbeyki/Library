using AppFramework.Application.Email;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Areas.Identity.Controllers;

[Area("Identity")]
public class EmailsController : Controller
{
    private readonly IEmailService _email;

    public EmailsController( IEmailService email)
    {
        _email = email;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public IActionResult SendE()
    {
        return View();
    }
    [HttpPost]
    public IActionResult SendE(EmailModel email)
    {
        _email.Send(email);
        return View();
    }

    //[HttpGet]
    //public IActionResult SendMail()
    //{
    //    return View();
    //}
    //[HttpPost]
    //public async Task<IActionResult> SendMail([FromForm]MailData mailData)
    //{
    //    bool result = await _mail.SendAsync(mailData, new CancellationToken());

    //    if (result)
    //    {
    //        return StatusCode(ViewBag.Message = "Mail has successfully been sent.");
    //    }
    //    else
    //    {
    //        return StatusCode(ViewBag.Message =  "An error occured. The Mail could not be sent.");
    //    }
    //}

}
