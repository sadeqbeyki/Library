using LibBook.DomainContracts.Book;
using LibBook.DomainContracts.Borrow;
using LibIdentity.DomainContracts.UserContracts;

namespace Library.EndPoint.Areas.adminPanel.Models;

public class CreateBorrowViewModel
{
    public int BookId { get; set; }
    public string MemberId { get; set; }
    public string EmployeeId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime IdealReturnDate { get; set; }
    public string ReturnEmployeeId { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string Description { get; set; }
    public List<UserViewModel> Members { get; set; }
    public List<BookViewModel> Books { get; set; }
}

public class UpdateBorrowViewModel
{
    public BorrowDto Borrow { get; set; }
    public List<UserViewModel> Members { get; set; }
    public List<BookViewModel> Books { get; set; }
}

