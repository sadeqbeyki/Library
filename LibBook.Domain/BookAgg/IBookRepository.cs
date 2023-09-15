using AppFramework.Domain;
using LibBook.DomainContracts.Book;

namespace LibBook.Domain.BookAgg;

public interface IBookRepository : IRepository<Book, int>
{
    Task<List<BookViewModel>> GetBooks();
    //EditInventory GetDetails(long id);
    //Inventory GetBy(long productId);
    //List<InventoryViewModel> Search(InventorySearchModel searchModel);
    //List<InventoryOperationViewModel> GetOperationLog(long inventoryId);

    //Task<BookViewModel> GetBookById(int id);
}
