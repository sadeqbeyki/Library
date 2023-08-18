using System.Collections.Generic;

namespace Library.EndPoint.Areas.Identity.Models;

public class MailData
{
    // Receiver
    public List<string> To { get; }
    public List<string> Bcc { get; }

    public List<string> Cc { get; }

    // Sender
    public string From { get; }

    public string DisplayName { get; }

    public string ReplyTo { get; }

    public string ReplyToName { get; }

    // Content
    public string Subject { get; }

    public string Body { get; }
    //Attachement
    public IFormFileCollection? Attachments { get; set; }

    public MailData(List<string> to, List<string> bcc, List<string> cc, string from, string displayName, string replyTo, string replyToName, string subject, string body)
    {
        To = to;
        Bcc = bcc;
        Cc = cc;
        From = from;
        DisplayName = displayName;
        ReplyTo = replyTo;
        ReplyToName = replyToName;
        Subject = subject;
        Body = body;
    }


    //public MailData(List<string> to, string subject, string? body = null, string? from = null, string? displayName = null,
    //    string? replyTo = null, string? replyToName = null, List<string>? bcc = null, List<string>? cc = null)
    //{
    //    To = to;
    //    Subject = subject;
    //    Body = body;
    //    From = from;
    //    DisplayName = displayName;
    //    ReplyTo = replyTo;
    //    ReplyToName = replyToName;
    //    Bcc = bcc ?? new List<string>();
    //    Cc = cc ?? new List<string>();
    //}
}



