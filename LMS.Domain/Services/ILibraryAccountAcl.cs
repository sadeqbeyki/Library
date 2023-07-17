namespace LMS.Domain.Services;

public interface ILibraryAccountAcl
{
    (string name, string email) GetAccountBy(string id);
}
