using AppFramework.Domain;
using LMS.Domain.BookAgg;

namespace LMS.Domain.BorrowAgg;

public class Borrow : BaseEntity
{
    public int BookId { get; set; }
    public Book Book { get; set; }
    public string MemberID { get; set; }
    public string EmployeeId { get; set; }
    //public DateTime LendDate { get; set; }
    public DateTime IdealReturnDate { get; set; }
    public string ReturnEmployeeID { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string Description { get; set; }


    public Borrow(int bookId, string memberID, string employeeId, DateTime idealReturnDate, string returnEmployeeID, DateTime returnDate, string description)
    {
        BookId = bookId;
        MemberID = memberID;
        EmployeeId = employeeId;
        IdealReturnDate = idealReturnDate;
        ReturnEmployeeID = returnEmployeeID;
        ReturnDate = returnDate;
        Description = description;
    }
    public void Edit(int bookId, string memberID, string employeeId, DateTime idealReturnDate, string returnEmployeeID, DateTime returnDate, string description)
    {
        BookId = bookId;
        MemberID = memberID;
        EmployeeId = employeeId;
        IdealReturnDate = idealReturnDate;
        ReturnEmployeeID = returnEmployeeID;
        ReturnDate = returnDate;
        Description = description;
    }
}
