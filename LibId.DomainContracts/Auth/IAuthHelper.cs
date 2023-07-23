namespace LibIdentity.DomainContracts.Auth
{
    public interface IAuthHelper
    {
        void SignOut();
        bool IsAuthenticated();
        void SignIn(AuthViewModel account);
        string CurrentAccountRole();
        AuthViewModel CurrentAccountInfo();
        List<int> GetPermissions();
        string CurrentAccountId();
        string CurrentAccountMobile();
    }
}
