using LibIdentity.Domain.UserAgg;
using Microsoft.AspNetCore.Identity;

namespace LibIdentity.DomainContracts.UserContracts
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUser(CreateUserViewModel model);
        Task<IdentityResult> Register(CreateUserViewModel model);
        Task<IdentityResult> DeleteUser(int id);
        Task<UpdateUserViewModel> GetUser(int id);
        Task<List<UpdateUserViewModel>> GetUsers();
        Task<IdentityResult> Update(UpdateUserViewModel user);

        UpdateUserViewModel GetAccountBy(int id);

        Task<List<UserRolesViewModel>> GetAllUsers();


    }
}