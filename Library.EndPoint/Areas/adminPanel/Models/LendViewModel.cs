using LibBook.DomainContracts.Borrow;

namespace Library.EndPoint.Areas.adminPanel.Models;

public class LendViewModel
{
    public List<LoanDto> Loans { get; set; } = new List<LoanDto>();
    public LoanSearchModel SearchModel { get; set; } = new LoanSearchModel();
}
