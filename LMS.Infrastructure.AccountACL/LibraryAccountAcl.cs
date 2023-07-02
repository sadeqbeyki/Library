using LI.ApplicationContracts.UserContracts;
using LMS.Domain.Services;

namespace LMS.Infrastructure.AccountACL
{
    public class LibraryAccountAcl : ILibraryAccountAcl
    {
        private readonly IUserService _userService;

        public LibraryAccountAcl(IUserService userService)
        {
            _userService = userService;
        }

        public (string name, string mobile) GetAccountBy(string id)
        {
            var account = _userService.GetUser(id);
            return (account.name, account.mobile);
        }
    }
}