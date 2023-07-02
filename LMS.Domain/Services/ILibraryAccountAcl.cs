namespace LMS.Domain.Services;

public interface ILibraryAccountAcl
{
    (string name, string mobile) GetAccountBy(string id);
}
