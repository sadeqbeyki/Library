using LendBook.Domain.Services;
using LI.ApplicationContracts.UserContracts;

namespace LendBook.Infrastructure.AccountACL;

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