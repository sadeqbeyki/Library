namespace LibBook.Domain.Services;

public interface ILibraryIdentityAcl
{
    Task<(string name, string email)> GetAccountBy(string id, CancellationToken cancellationToken);
    Task<string> GetUserName(string id);
}
