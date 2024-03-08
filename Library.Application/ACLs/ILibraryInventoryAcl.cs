using Library.Domain.Entities.LendAgg;

namespace Library.Application.ACLs;

public interface ILibraryInventoryAcl
{
    //bool LendFromInventory(List<LendItem> item);
    Task<bool> BorrowFromInventory(Lend lend);
    Task<bool> ReturnToInventory(Lend lend);
}