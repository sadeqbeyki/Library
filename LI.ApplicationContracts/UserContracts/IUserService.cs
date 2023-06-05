using Microsoft.AspNetCore.Identity;

namespace LI.ApplicationContracts.UserContracts
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUser(UserDto model);
        Task<IdentityResult> DeleteUser(int id);
        Task<UserDto> GetUser(string id);
        Task<List<UserDto>> GetUsers();
        Task<IdentityResult> UpdateUser(UserDto user);
    }
}