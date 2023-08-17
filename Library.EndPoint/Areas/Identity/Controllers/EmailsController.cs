using Library.EndPoint.Areas.Identity.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace Library.EndPoint.Areas.Identity.Controllers;

[Area("Identity")]
public class EmailsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public IActionResult SendEmail()
    {
        return View();
    }
    [HttpPost]
    public IActionResult SendEmail(EmailModel model)
    {
        using (MailMessage mailMessage = new(model.Email, model.To))
        {
            mailMessage.Subject = model.Subject;
            mailMessage.Body = model.Body;
            //if (model.Attachment.Length > 0)
            //{
            //    string fileName = Path.GetFileName(model.Attachment.FileName);
            //    mailMessage.Attachments.Add(new Attachment(model.Attachment.OpenReadStream(), fileName));
            //}
            mailMessage.IsBodyHtml = false;
            using SmtpClient smtp = new();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential NetworkCred = new(model.Email, model.Password);
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mailMessage);
            ViewBag.Message = "Email sent.";
        }

        return View("SendEmail");
    }
}
