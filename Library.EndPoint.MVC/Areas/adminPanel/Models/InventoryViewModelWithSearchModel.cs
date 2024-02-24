using Library.EndPoint.MVC.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Warehouse.Application.DTOs;

namespace Library.EndPoint.MVC.Areas.adminPanel.Models;

public class InventoryViewModelWithSearchModel
{
    public PaginatedList<InventoryViewModel> Inventory { get; set; }
    public InventorySearchModel SearchModel { get; set; }
    public List<SelectListItem> Books { get; set; }
}
