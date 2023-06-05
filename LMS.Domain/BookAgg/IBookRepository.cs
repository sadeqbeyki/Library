using AppFramework.Domain;
using BI.ApplicationContracts.Inventory;
using BI.Domain.InventoryAgg;

namespace LMS.Domain.BookAgg;

public interface IBookRepository : IRepository<Book>
{
    //EditInventory GetDetails(long id);
    //Inventory GetBy(long productId);
    //List<InventoryViewModel> Search(InventorySearchModel searchModel);
    //List<InventoryOperationViewModel> GetOperationLog(long inventoryId);
}
