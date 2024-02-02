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

    public async Task<(string name, string email)> GetAccountBy(Guid id, CancellationToken cancellationToken)
    {
        var account = await _userService.GetUserByIdAsync(id, cancellationToken);
        return (account.FirstName, account.Email);
    }
    public async Task<string> GetUserName(Guid id)
    {
        var userName = await _userService.GetUserNameAsync(id);
        return userName;
    }

    public Guid GetCurrentUserId()
    {
        var userId = _userService.GetCurrentUserId();
        return userId;
    }
}