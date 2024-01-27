using Identity.Application.DTOs.User;
using Identity.Application.Interfaces.Base;
using Identity.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Interfaces;

public interface IUserService : IServiceBase
{
    //User
    Task<IdentityResult> Register(CreateUserDto model);
    Task<(bool isSucceed, string userId)> CreateUserAsync(CreateUserDto userDto);
    Task<bool> UpdateUserAsync(UpdateUserDto userDto);
    Task<bool> DeleteUserAsync(string userId);

    //Get
    Task<UserDetailsDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken);
    Task<string> GetUserNameAsync(string userId);
    Task<ApplicationUser> GetUserByNameAsync(string userName);
    Task<UserDetailsDto> GetUserByUserNameAsync(string userName);
    Task<string> GetUserIdAsync(string userName);
    Task<bool> IsUniqueUserName(string userName);
    Task<ApplicationUser?> GetMember(string id, CancellationToken cancellationToken);
    Task<List<UserDetailsDto>> GetAllUsersAsync();

    //User Roles
    Task<bool> IsInRoleAsync(string userId, string role);
    Task<bool> AssignUserToRole(string userName, IList<string> roles);
    Task<bool> UpdateUsersRole(string userName, IList<string> usersRole);
    Task<List<string>> GetUserRolesAsync(string userId);

    Task<string> AssignRoleAsync(string userId, string roles);
}
