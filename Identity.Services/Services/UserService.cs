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
using Identity.Domain.Entities.Role;
using FluentValidation.Results;


namespace Identity.Services.Services;

public class UserService : ServiceBase<UserService>, IUserService
{
    private new readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IDistributedCache _distributedCache;
    private readonly IEmailService _emailService;

    private readonly RoleManager<ApplicationRole> _roleManager;


    public UserService(
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        IDistributedCache distributedCache,
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        IEmailService emailService,

        RoleManager<ApplicationRole> roleManager) : base(serviceProvider)
    {
        _userManager = userManager;
        _mapper = mapper;
        _distributedCache = distributedCache;
        _configuration = configuration;
        _emailService = emailService;

        _roleManager = roleManager;
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

    public async Task<List<UserRolesDto>> GetUserWithRoles()
    {
        var users = await _userManager.Users.ToListAsync()
            ?? throw new NotFoundException("User not found");

        var usersMap = _mapper.Map<List<UserRolesDto>>(users);
        foreach (var user in usersMap)
        {
            user.Roles = await GetUserRolesAsync(user.UserId);
        }

        return usersMap;
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
        var existingUser = await _userManager.FindByEmailAsync(model.Email)
            ?? await _userManager.FindByNameAsync(model.UserName);
        if (existingUser != null)
        {
            throw new BadRequestException($"Your chosen '{existingUser}' is already registered on the site");
        }

        //create user
        var userMap = _mapper.Map<ApplicationUser>(model);
        userMap.Id = Guid.NewGuid().ToString();
        var result = await _userManager.CreateAsync(userMap, model.Password)
            ?? throw new BadRequestException("cant add new user");

        var addUserRole = await _userManager.AddToRolesAsync(userMap, new List<string> { "Member" })
            ?? throw new BadRequestException("cant add user to roles");

        //confirmation email 
        bool sendConfirmMail = SendConfirmationLink(userMap);

        return result;
    }

    public async Task<(bool isSucceed, string userId)> CreateUserAsync(CreateUserDto model)
    {
        var existingUser = await _userManager.FindByEmailAsync(model.Email)
            ?? await _userManager.FindByNameAsync(model.UserName);
        if (existingUser != null)
        {
            var error = new IdentityError
            {
                Code = "Duplicate Email",
                Description = "This email already exists on the website."
            };
            return (isSucceed: false, userId: existingUser.Id);
        }
        var user = _mapper.Map<ApplicationUser>(model);
        user.Id = Guid.NewGuid().ToString();
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            //throw new ValidationException(string.Join("\n", errors));
        }

        //confirm user
        user.EmailConfirmed = true;
        model.Roles = new List<string> { "Member" };

        var addUserRole = await _userManager.AddToRolesAsync(user, model.Roles);
        if (!addUserRole.Succeeded)
        {
            var validationFailures = addUserRole.Errors.Select(e => new ValidationFailure("", e.Description));
            throw new ValidationException(validationFailures);
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

        var isUserAdmin = await _userManager.IsInRoleAsync(user, "Admin");
        if (isUserAdmin)
        {
            throw new BadRequestException("You can not delete system or admin user");
        }
        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }
    #endregion

    #region UserRole
    public async Task<string> AssignRoleAsync(string userId, string roleId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId)
                ?? throw new NotFoundException("User not found");

        var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId)
                ?? throw new NotFoundException("Role not found");

        if (await _userManager.IsInRoleAsync(user, role.Name))
            return "User is already in the selected role.";

        if (await AssignUserToRole(user.UserName, new List<string> { role.Name }))
            return "Role assigned to User";
        return "There is a problem in assigning a role to a user";
    }

    public async Task<bool> RemoveUserRole(string userId, string roleId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var role = await _roleManager.FindByIdAsync(roleId);

        if (user == null || role == null)
            throw new BadRequestException("cant find user or role!");

        if (role.Name == "Member")
            throw new BadRequestException("You cannot remove 'Member' role from users");

        var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
        if (!result.Succeeded)
            return false;
        return true;
    }


    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId)
            ?? throw new NotFoundException("User not found");

        var result = await _userManager.IsInRoleAsync(user, role);
        return result;
    }
    public async Task<bool> IsInRoles(string userId, List<string> roles)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId)
            ?? throw new NotFoundException("User not found");

        bool isIn = false;
        foreach (var role in roles)
        {
            isIn = await _userManager.IsInRoleAsync(user, role);
            if (isIn)
                break;
        }
        return isIn;
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

        IdentityResult result = await _userManager.AddToRolesAsync(user, roles);
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
    private bool SendConfirmationLink(ApplicationUser user)
    {
        var emailConfirmToken = _userManager.GenerateEmailConfirmationTokenAsync(user);

        var scheme = _httpContextAccessor.HttpContext?.Request.Scheme ?? "https";
        var confirmationLink = /*Url.Action*/(nameof(ConfirmEmail), "Auth", new { emailConfirmToken, email = user.Email }, scheme);

        EmailModel message = new()
        {
            FromName = "Library Manager",
            FromAddress = "info@library.com",
            ToName = user.UserName,
            ToAddress = user.Email,
            Subject = "Confirm Your Registration",
            Content = "Please click the following link to confirm your registration: <a href=\"" + confirmationLink + "\">Confirm</a>"
        };
        _emailService.Send(message);
        return true;
    }

    private async Task<string> ConfirmEmail(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email)
            ?? throw new NotFoundException("User not found.");

        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded ? nameof(ConfirmEmail) : "Error";
    }

    public async Task<string> GetConfirmEmailToken(string userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
        var emailConfirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return emailConfirmToken;
    }
    #endregion

}
