using LibInventory.DomainContracts.Inventory;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.EndPoint.Areas.adminPanel.Models;

public class UpdateInventoryViewModel
{
    public EditInventory Inventory { get; set; }
    public List<SelectListItem> Books { get; set; }

}
