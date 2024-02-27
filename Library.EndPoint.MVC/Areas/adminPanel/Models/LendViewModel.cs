using Library.Application.DTOs.Lend;
using Library.EndPoint.MVC.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.EndPoint.MVC.Areas.adminPanel.Models;

public class LendViewModel
{
    public PaginatedList<LendDto> Loans { get; set; }
    public LendSearchModel SearchModel { get; set; } = new LendSearchModel();
    public List<SelectListItem> Books { get; set; } = new List<SelectListItem>();
}
