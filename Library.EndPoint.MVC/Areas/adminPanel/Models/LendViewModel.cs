using LibBook.DomainContracts.Borrow;
using Library.EndPoint.MVC.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.EndPoint.MVC.Areas.adminPanel.Models;

public class LendViewModel
{
    public PaginatedList<LoanDto> Loans { get; set; }
    public LoanSearchModel SearchModel { get; set; } = new LoanSearchModel();
    public List<SelectListItem> Books { get; set; } = new List<SelectListItem>();
}
