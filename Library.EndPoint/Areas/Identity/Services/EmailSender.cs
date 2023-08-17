using Library.EndPoint.Areas.Identity.Models;
using Newtonsoft.Json;
using System.Text;

namespace Library.EndPoint.Areas.Identity.Services;



public class EmailSender
{
    private readonly string _apiKey;

    public EmailSender(string apiKey)
    {
        _apiKey = apiKey;
    }

    
}