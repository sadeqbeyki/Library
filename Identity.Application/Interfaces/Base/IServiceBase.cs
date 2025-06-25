using Identity.Domain.Entities.User;

namespace Identity.Application.Interfaces.Base;

public interface IServiceBase
{
    Guid GetCurrentUserId();
    Task<ApplicationUser> GetCurrentUser();
    Task<IList<string>> GetCurrentUserRoles();
}
