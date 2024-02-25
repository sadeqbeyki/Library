using Microsoft.AspNetCore.Mvc.Rendering;
using Warehouse.Application.DTOs.Inventory;

namespace Library.EndPoint.MVC.Areas.adminPanel.Models;

public class UpdateInventoryViewModel
{
    public EditInventory Inventory { get; set; }
    public List<SelectListItem> Books { get; set; }

}
