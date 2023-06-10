using Microsoft.AspNetCore.Identity;

namespace LI.ApplicationContracts.UserContracts
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUser(UserDto model);
        Task<IdentityResult> DeleteUser(int id);
        Task<UpdateUserDto> GetUser(string id);
        Task<List<UpdateUserDto>> GetUsers();
        Task<IdentityResult> UpdateUser(UpdateUserDto user);
    }
}