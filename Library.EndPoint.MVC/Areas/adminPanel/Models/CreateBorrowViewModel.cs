using Identity.Application.DTOs.User;
using Library.Application.DTOs.Book;
using Library.Application.DTOs.Lends;
using System.ComponentModel.DataAnnotations;

namespace Library.EndPoint.MVC.Areas.adminPanel.Models;

public class CreateBorrowViewModel
{
    [Required(ErrorMessage = "Book Name field cannot be empty!")]
    public int BookId { get; set; }
    [Required(ErrorMessage = "Username field cannot be empty!")]
    public string MemberId { get; set; }
    public string EmployeeId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime IdealReturnDate { get; set; }
    public string ReturnEmployeeId { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string Description { get; set; }
    public List<UserDetailsDto> Members { get; set; }
    public List<BookViewModel> Books { get; set; }
}

public class UpdateBorrowViewModel
{
    public LendDto Lend { get; set; }
    public List<UserDetailsDto> Members { get; set; }
    public List<BookViewModel> Books { get; set; }
}

public class LoanViewModel
{
    public int BookId { get; set; }
    public string MemberId { get; set; }
}
