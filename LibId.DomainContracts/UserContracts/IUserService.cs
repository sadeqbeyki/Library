using Microsoft.AspNetCore.Identity;

namespace LibIdentity.DomainContracts.UserContracts
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUser(CreateUserViewModel model);
        Task<IdentityResult> Register(CreateUserViewModel model);
        Task<IdentityResult> DeleteUser(int id);
        Task<IdentityResult> Update(UpdateUserViewModel user);

        Task<UpdateUserViewModel> GetUser(int id);
        UpdateUserViewModel GetAccountBy(int id);
        Task<string> GetUserNameByIdAsync(string userId);
        Task<List<UpdateUserViewModel>> GetUsers();
        Task<List<UserRolesViewModel>> GetAllUsers();


    }
}