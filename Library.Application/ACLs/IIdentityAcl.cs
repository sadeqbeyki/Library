namespace Library.Application.ACLs;

public interface IIdentityAcl
{
    Task<(string name, string email)> GetAccountBy(Guid id, CancellationToken cancellationToken);
    Task<string> GetUserName(Guid? id);
    Guid GetCurrentUserId();
    //Task<List<UserDetailsDto>> GetAllUsersAsync();

}
