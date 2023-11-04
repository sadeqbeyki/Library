using System.ComponentModel.DataAnnotations;

namespace LibBook.DomainContracts.Borrow;

public class BorrowDto
{
    public int Id { get; set; }
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
}
