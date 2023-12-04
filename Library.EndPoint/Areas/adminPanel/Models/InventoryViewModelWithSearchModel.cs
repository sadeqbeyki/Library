using LibInventory.DomainContracts.Inventory;
using Library.EndPoint.Tools;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.EndPoint.Areas.adminPanel.Models;

public class InventoryViewModelWithSearchModel
{
    public PaginatedList<InventoryViewModel> Inventory { get; set; }
    public InventorySearchModel SearchModel { get; set; }
    public List<SelectListItem> Books { get; set; }
}
