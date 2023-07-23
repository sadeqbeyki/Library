using LibBook.Domain.Services;
using LibIdentity.DomainContracts.UserContracts;

namespace LibBook.Infrastructure.AccountACL;

public class LibraryIdentityAcl : ILibraryIdentityAcl
{
    private readonly IUserService _userService;

    public LibraryIdentityAcl(IUserService userService)
    {
        _userService = userService;
    }

    public (string name, string email) GetAccountBy(string id)
    {
        var account = _userService.GetAccountBy(id);
        return (account.FirstName, account.Email);
    }
}