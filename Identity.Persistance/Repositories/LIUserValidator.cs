using Identity.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace Identity.Persistance.Repositories;

public class LIUserValidator : IUserValidator<ApplicationUser>
{
    public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> userManager, ApplicationUser user)
    {
        List<IdentityError> errors = new();

        //if (!user.Email.EndsWith("@xserver.com"))
        //{
        //    errors.Add(new IdentityError
        //    {
        //        Code = "InvalidEmail",
        //        Description = "Use xServer email for Registration"
        //    });
        //}

        return Task.FromResult(errors.Any() ?
                IdentityResult.Failed(errors.ToArray()) :
                IdentityResult.Success);
    }
}
