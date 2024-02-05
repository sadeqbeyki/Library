using Identity.Application.DTOs.User;
using Identity.Application.Interfaces.Base;
using Identity.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Interfaces;

public interface IUserService : IServiceBase
{
    //User
    Task<(Guid userId, string emailConfirmToken)> Register(CreateUserDto model);
    Task<(bool isSucceed, Guid userId)> CreateUserAsync(CreateUserDto userDto);
    Task<bool> UpdateUserAsync(UpdateUserDto userDto);
    Task<bool> DeleteUserAsync(Guid userId);

    //Get
    Task<UserDetailsDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<string> GetUserNameAsync(Guid userId);
    Task<ApplicationUser> GetUserByNameAsync(string userName);
    Task<UserDetailsDto> GetUserByUserNameAsync(string userName);
    Task<string> GetUserIdAsync(string userName);
    Task<bool> IsUniqueUserName(string userName);
    Task<ApplicationUser?> GetMember(Guid id, CancellationToken cancellationToken);
    Task<List<UserDetailsDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<UserDetailsDto>> GetAllUsersAsync();

    //User Roles
    Task<bool> IsInRoleAsync(string userId, string roleName);
    Task<bool> IsInRoles(string userId, List<string> roles);
    Task<bool> AssignUserToRole(string userName, IList<string> roles);
    Task<bool> UpdateUsersRole(string userName, IList<string> usersRole);
    Task<List<string>> GetUserRolesAsync(Guid userId);

    Task<string> AssignRoleAsync(Guid userId, Guid roles);
    Task<bool> RemoveUserRole(Guid userId, Guid roleId);
    Task<List<UserRolesDto>> GetUserWithRoles();

    //email
    Task<IdentityResult> ConfirmEmail(string token, string email);
}
