using AppFramework.Domain;

namespace LendBook.Domain.LendAgg;

public interface ILendRepository : IRepository<Lend>
{
    Lend GetBookBy(Guid bookId);
}
