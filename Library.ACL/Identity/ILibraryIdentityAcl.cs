using Identity.Application.DTOs.User;

namespace Library.ACL.Identity;

public interface ILibraryIdentityAcl
{
    Task<(string name, string email)> GetAccountBy(Guid id, CancellationToken cancellationToken);
    Task<string> GetUserName(Guid? id);
    Guid GetCurrentUserId();
    Task<List<UserDetailsDto>> GetAllUsersAsync();

}
