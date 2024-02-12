using System.ComponentModel.DataAnnotations;

namespace LibBook.DomainContracts.Borrow;

public class LoanDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    [Required(ErrorMessage = "Book Name field cannot be empty!")]
    public string BookTitle { get; set; }
    [Required(ErrorMessage = "Username field cannot be empty!")]
    public Guid MemberId { get; set; }
    public string MemberName { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime IdealReturnDate { get; set; }
    public Guid? ReturnEmployeeID { get; set; }
    public string ReturnEmployeeName { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string Description { get; set; }
}


