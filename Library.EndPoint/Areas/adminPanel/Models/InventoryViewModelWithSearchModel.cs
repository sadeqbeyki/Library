using BI.ApplicationContracts.Inventory;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.EndPoint.Areas.adminPanel.Models;

public class InventoryViewModelWithSearchModel
{
    public List<InventoryViewModel> Inventory { get; set; }
    public InventorySearchModel SearchModel { get; set; }
    public List<SelectListItem> Books { get; set; }
}
