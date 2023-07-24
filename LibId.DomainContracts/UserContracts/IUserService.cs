using Microsoft.AspNetCore.Identity;

namespace LibIdentity.DomainContracts.UserContracts
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUser(UserDto model);
        Task<IdentityResult> DeleteUser(int id);
        Task<UpdateUserDto> GetUser(int id);
        Task<List<UpdateUserDto>> GetUsers();
        Task<IdentityResult> Update(UpdateUserDto user);

        UpdateUserDto GetAccountBy(int id);

    }
}