using Identity.Application.Interfaces;
using LibBook.Domain.Services;

namespace LibBook.Infrastructure.AccountACL;

public class LibraryIdentityAcl : ILibraryIdentityAcl
{
    private readonly IUserService _userService;

    public LibraryIdentityAcl(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<(string name, string email)> GetAccountBy(string id, CancellationToken cancellationToken)
    {
        var account = await _userService.GetUserByIdAsync(id, cancellationToken);
        return (account.FirstName, account.Email);
    }
    public async Task<string> GetUserName(string id)
    {
        var userName = await _userService.GetUserNameAsync(id);
        return userName;
    }
}