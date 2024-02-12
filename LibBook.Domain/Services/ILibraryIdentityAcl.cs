namespace LibBook.Domain.Services;

public interface ILibraryIdentityAcl
{
    Task<(string name, string email)> GetAccountBy(Guid id, CancellationToken cancellationToken);
    Task<string> GetUserName(Guid? id);
    Guid GetCurrentUserId();
}
