using Microsoft.AspNetCore.Identity;

namespace LibIdentity.DomainContracts.UserContracts
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUser(UserDto model);
        Task<IdentityResult> DeleteUser(int id);
        Task<UserViewModel> GetUser(int id);
        Task<List<UserViewModel>> GetUsers();
        Task<IdentityResult> Update(UserViewModel user);

        UserViewModel GetAccountBy(int id);

    }
}