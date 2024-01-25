using Identity.Domain.Entities.User;

namespace Identity.Application.Interfaces.Base
{
    public interface IServiceBase
    {
        string GetCurrentUserId();
        Task<ApplicationUser> GetCurrentUser();
        Task<IList<string>> GetCurrentUserRoles();
    }
}
