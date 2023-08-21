using LibIdentity.Domain.UserAgg;
using Microsoft.AspNetCore.Identity;

namespace LibIdentity.Infrastructure.Repositories
{
    public class LIUserValidator : IUserValidator<UserIdentity>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<UserIdentity> manager, UserIdentity user)
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
}
