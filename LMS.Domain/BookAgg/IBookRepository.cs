using AppFramework.Domain;
using BI.ApplicationContracts.Inventory;
using BI.Domain.InventoryAgg;
using LMS.Contracts.Book;

namespace LMS.Domain.BookAgg;

public interface IBookRepository : IRepository<Book, int>
{
    Task<List<BookViewModel>> GetBooks();
    //EditInventory GetDetails(long id);
    //Inventory GetBy(long productId);
    //List<InventoryViewModel> Search(InventorySearchModel searchModel);
    //List<InventoryOperationViewModel> GetOperationLog(long inventoryId);
}
