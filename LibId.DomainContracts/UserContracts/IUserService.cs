using Microsoft.AspNetCore.Identity;

namespace LibIdentity.DomainContracts.UserContracts
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUser(UserDto model);
        Task<IdentityResult> DeleteUser(string id);
        Task<UpdateUserDto> GetUser(string id);
        Task<List<UpdateUserDto>> GetUsers();
        Task<IdentityResult> Update(UpdateUserDto user);

        UpdateUserDto GetAccountBy(string id);

    }
}