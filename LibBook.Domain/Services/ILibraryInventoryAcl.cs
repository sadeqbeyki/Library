using LibBook.Domain.BorrowAgg;

namespace LibBook.Domain.Services;

public interface ILibraryInventoryAcl
{
    //bool LendFromInventory(List<LendItem> item);
    bool LoanFromInventory(Borrow borrow);
    bool ReturnToInventory(Borrow borrow);
}