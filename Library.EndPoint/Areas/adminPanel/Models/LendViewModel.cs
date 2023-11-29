using LibBook.DomainContracts.Borrow;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.EndPoint.Areas.adminPanel.Models;

public class LendViewModel
{
    public List<LoanDto> Loans { get; set; } = new List<LoanDto>();
    public LoanSearchModel SearchModel { get; set; } = new LoanSearchModel();
    public List<SelectListItem> Books { get; set; } = new List<SelectListItem>();
}
