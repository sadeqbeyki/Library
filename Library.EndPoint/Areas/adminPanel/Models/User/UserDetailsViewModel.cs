using Identity.Application.Common.Enums;

namespace Library.EndPoint.Areas.adminPanel.Models.User;

public class UserDetailsViewModel
{
    public string Id { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime? BirthDate { get; set; }

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public SupportedCulture Culture { get; set; }

    public DateTime? JoinedOn { get; set; }

    public IList<string> Roles { get; set; } = new List<string>();

    //public string ReturnUrl { get; set; } = "/";
}
