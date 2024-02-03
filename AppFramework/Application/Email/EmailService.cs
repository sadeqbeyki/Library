using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace AppFramework.Application.Email;

public class EmailService : IEmailService
{
    public void Send(EmailModel mail)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(mail.FromName, mail.FromAddress));
        message.To.Add(new MailboxAddress(mail.ToName, mail.ToAddress));

        message.Subject = mail.Subject;

        message.Body = new TextPart(TextFormat.Html)
        {
            Text = mail.Content
        };

        using SmtpClient emailClient = new();

        emailClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
        emailClient.Authenticate("harzgat@gmail.com", "edvoowcfdoyyoatw");
        emailClient.Send(message);

        emailClient.Disconnect(true);
    }
}






