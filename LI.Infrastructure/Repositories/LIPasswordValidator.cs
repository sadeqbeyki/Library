using LI.Domain.UserAgg;
using Microsoft.AspNetCore.Identity;

namespace LI.Infrastructure.Repositories
{
    //Interface Phone Book Password Validator
    public class LIPasswordValidator : PasswordValidator<User>
    {
        private readonly LiIdentityDbContext _userDbContext;

        public LIPasswordValidator(LiIdentityDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }
        public override Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            var parentResult = base.ValidateAsync(manager, user, password).Result;
            List<IdentityError> errors = new();
            if (!parentResult.Succeeded)
            {
                errors = parentResult.Errors.ToList();
            }
            if (user.UserName == password || user.UserName.Contains(password))
            {
                errors.Add(new IdentityError
                {
                    Code = "Password",
                    Description = "Password is equal to username"
                });
            }
            if (_userDbContext.BadPasswords.Any(c => c.Passwrod == password))
            {
                errors.Add(new IdentityError
                {
                    Code = "Password",
                    Description = "You can not select password from bad password List"
                });
            }
            return Task.FromResult(errors.Any() ?
                IdentityResult.Failed(errors.ToArray()) :
                IdentityResult.Success
                );
        }
    }
}
