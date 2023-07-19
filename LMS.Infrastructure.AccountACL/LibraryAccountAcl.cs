using LI.ApplicationContracts.UserContracts;
using LMS.Domain.Services;

namespace LMS.Infrastructure.AccountACL;

public class LibraryAccountAcl : ILibraryAccountAcl
{
    private readonly IUserService _userService;

    public LibraryAccountAcl(IUserService userService)
    {
        _userService = userService;
    }

    public (string name, string email) GetAccountBy(string id)
    {
        var account = _userService.GetAccountBy(id);
        return (account.FirstName, account.Email);
    }
}