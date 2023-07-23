using LibBook.Domain.BorrowAgg;
using LibBook.Domain.LendAgg;

namespace LibBook.Domain.Services;

public interface ILibraryInventoryAcl
{
    bool LendFromInventory(List<LendItem> item);
    bool BorrowFromInventory(Borrow item);
}