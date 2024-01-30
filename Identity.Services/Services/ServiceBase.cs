using AppFramework.Application.Email;
using Identity.Application.Common.Const;
using Identity.Application.Common.Exceptions;
using Identity.Application.Interfaces.Base;
using Identity.Domain.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.Services.Services;

public class ServiceBase<TService> : IServiceBase where TService : class
{
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly UserManager<ApplicationUser> _userManager;


    protected ServiceBase(IServiceProvider serviceProvider)
    {
        _httpContextAccessor = (IHttpContextAccessor)serviceProvider.GetService(typeof(IHttpContextAccessor));
        _userManager = (UserManager<ApplicationUser>)serviceProvider.GetService(typeof(UserManager<ApplicationUser>));

    }


    public string GetCurrentUserId()
    {
        ClaimsIdentity identity = _httpContextAccessor?.HttpContext?.User?.Identity as ClaimsIdentity;
        string userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userId ?? string.Empty;

        //    return _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    //return _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }


    public async Task<ApplicationUser> GetCurrentUser()
    {
        // Obtain MailId from token
        ClaimsIdentity identity = _httpContextAccessor?.HttpContext?.User?.Identity as ClaimsIdentity;
        var userMailId = identity?.FindFirst(AppConstants.JWT_SUB)?.Value;

        // Obtain user from token
        ApplicationUser user = null;
        if (!string.IsNullOrEmpty(userMailId))
        {
            user = await _userManager.FindByEmailAsync(userMailId);
        }

        return user;
    }

    public async Task<IList<string>> GetCurrentUserRoles()
    {
        ApplicationUser currentUser = await GetCurrentUser();
        IList<string> userRoles = await _userManager.GetRolesAsync(currentUser);

        if (userRoles.Count > 0)
        {
            return userRoles;
        }
        throw new NotFoundException("not found");
    }



}
