using AppFramework.Application.Email;
using AutoMapper;
using Identity.Application.Common.Exceptions;
using Identity.Application.DTOs.User;
using Identity.Application.Interfaces;
using Identity.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Identity.Application.Helper;
using LibIdentity.DomainContracts.Auth;

namespace Identity.Services.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly IEmailService _email;
    private readonly IConfiguration _configuration;

    private readonly IDistributedCache _distributedCache;

    public UserService(UserManager<ApplicationUser> userManager, IMapper mapper, IEmailService email, IDistributedCache distributedCache, IConfiguration configuration, IAuthService authService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _email = email;
        _distributedCache = distributedCache;
        _configuration = configuration;
        _authService = authService;
    }
    #region Get
    public async Task<UserDetailsDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId)
            ?? throw new NotFoundException("User not found");

        var userMap = _mapper.Map<UserDetailsDto>(user);
        userMap.Roles = await _userManager.GetRolesAsync(user);
        return userMap;
    }

    public async Task<string> GetUserNameAsync(string userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
        return user == null
            ? throw new NotFoundException("User not found")
            : await _userManager.GetUserNameAsync(user)
             ?? throw new NotFoundException("");
        //return user?.UserName ?? string.Empty;

    }

    public async Task<ApplicationUser> GetUserByNameAsync(string userName)
    {
        if (userName != null)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
                return user;
            throw new BadRequestException("user not found");
        }
        throw new BadRequestException("cant search for nul value!");
    }

    public async Task<UserDetailsDto> GetUserByUserNameAsync(string userName)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName)
            ?? throw new NotFoundException("User not found");
        var userMap = _mapper.Map<UserDetailsDto>(user);

        userMap.Roles = await _userManager.GetRolesAsync(user);
        return userMap;
    }

    public async Task<string> GetUserIdAsync(string userName)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.UserName == userName)
                ?? throw new NotFoundException("User not found");

        return await _userManager.GetUserIdAsync(user);
    }

    public async Task<bool> IsUniqueUserName(string userName)
    {
        return await _userManager.FindByNameAsync(userName) == null;
    }

    public async Task<ApplicationUser?> GetMember(string id, CancellationToken cancellationToken)
    {
        string key = $"member-{id}";
        ApplicationUser? member = await _distributedCache.GetObjectAsync<ApplicationUser>(key, cancellationToken);

        if (member is null)
        {
            member = await _userManager.FindByIdAsync(id);

            if (member is not null)
            {
                await _distributedCache.SetObjectAsync(key, member, _configuration, cancellationToken);
            }
        }

        return member;
    }

    #endregion

    #region GetAll
    public async Task<List<UserDetailsDto>> GetAllUsersAsync()
    {
        List<ApplicationUser> users = await _userManager.Users.ToListAsync();
        var result = _mapper.Map<List<UserDetailsDto>>(users);
        return result;
    }

    #endregion


    #region Create

    public async Task<IdentityResult> Register(CreateUserDto model)
    {
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            var error = new IdentityError
            {
                Code = "Duplicate Email",
                Description = "This email already exists on the website."
            };
            return IdentityResult.Failed(error);
        }
        //hash token
        //string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

        var userMap = _mapper.Map<ApplicationUser>(model);
        var result = await _userManager.CreateAsync(userMap, model.Password);

        //add to role
        await _userManager.AddToRoleAsync(userMap, "member");

        //confirmation email 
        var emailConfirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(userMap);
        var confirmationLink = (nameof(ConfirmEmail), "Account", new { emailConfirmToken, email = userMap.Email });

        EmailModel message = new()
        {
            FromName = "Library Manager",
            FromAddress = "info@library.com",
            ToName = userMap.UserName,
            ToAddress = userMap.Email,
            Subject = "Confirm Your Registration",
            Content = "Please click the following link to confirm your registration: <a href=\"" + confirmationLink + "\">Confirm</a>"
        };
        _email.Send(message);


        #region jwt 
        // Generate JWT token
        var token = _authService.GenerateJWTAuthetication(userMap);
        var validUserName = _authService.ValidateToken(token);
        // Set JWT token in the response
        //Response.Headers.Add("Authorization", "Bearer " + token);
        #endregion

        return result;
    }

    public async Task<(bool isSucceed, string userId)> CreateUserAsync(CreateUserDto userDto)
    {
        var user = _mapper.Map<ApplicationUser>(userDto);

        var result = await _userManager.CreateAsync(user, userDto.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            //throw new ValidationException(string.Join("\n", errors));
        }

        if (userDto.Roles == null
            || !userDto.Roles.Any()
            || userDto.Roles.All(string.IsNullOrWhiteSpace)
            || userDto.Roles.Contains("string"))
            userDto.Roles = new List<string> { "Member" };

        var addUserRole = await _userManager.AddToRolesAsync(user, userDto.Roles);
        if (!addUserRole.Succeeded)
        {
            var errors = addUserRole.Errors.Select(e => e.Description);
            //throw new ValidationException(string.Join("\n", errors));
        }
        return (result.Succeeded, user.Id);
    }
    #endregion

    #region Update
    public async Task<bool> UpdateUserAsync(UpdateUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.Id)
            ?? throw new NotFoundException("user not found");

        _mapper.Map(dto, user);
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new BadRequestException("cant update this user");
        }
        return result.Succeeded;
    }
    #endregion

    #region Delete
    public async Task<bool> DeleteUserAsync(string userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId)
            ?? throw new NotFoundException("User not found");

        var isUserAdmin = await _userManager.IsInRoleAsync(user, "admin");
        if (isUserAdmin)
        {
            throw new BadRequestException("You can not delete system or admin user");
        }
        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }
    #endregion

    #region UserRole
    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId)
            ?? throw new NotFoundException("User not found");

        var result = await _userManager.IsInRoleAsync(user, role);
        return result;
    }
    public async Task<List<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId)
            ?? throw new NotFoundException("User not found");
        var roles = await _userManager.GetRolesAsync(user);
        return roles.ToList();
    }
    public async Task<bool> AssignUserToRole(string userName, IList<string> roles)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName)
            ?? throw new NotFoundException("User not found");

        var result = await _userManager.AddToRolesAsync(user, roles);
        return result.Succeeded;
    }
    public async Task<bool> UpdateUsersRole(string userName, IList<string> usersRole)
    {
        var user = await _userManager.FindByNameAsync(userName)
            ?? throw new NotFoundException("cant find user"); ;
        var existingRoles = await _userManager.GetRolesAsync(user);
        var result = await _userManager.RemoveFromRolesAsync(user, existingRoles);
        result = await _userManager.AddToRolesAsync(user, usersRole);

        return result.Succeeded;
    }
    #endregion

    #region extention method
    private async Task<string> ConfirmEmail(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email)
            ?? throw new NotFoundException("User not found.");

        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded ? nameof(ConfirmEmail) : "Error";
    }
    #endregion
}
