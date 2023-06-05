using LI.Domain.UserAgg;
using Microsoft.AspNetCore.Identity;

namespace LI.Infrastructure.Repositories
{
    public class LIUserValidator : IUserValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
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
