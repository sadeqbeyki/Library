using AppFramework.Domain;

namespace LMS.Domain.LoanAgg;

public class Loan : BaseEntity
{
    public Guid BookId { get; set; }
    public string MemberID { get; set; }
    public string EmployeeId { get; set; }
    public string LoanDate { get; set; }
    public string IdealReturnDate { get; set; }
    public string ReturnEmployeeID { get; set; }
    public string ReturnDate { get; set; }
    public string Description { get; set; }

    public Loan(Guid bookId, string memberID, string employeeId, string loanDate, string idealReturnDate, string returnEmployeeID, string returnDate, string description)
    {
        BookId = bookId;
        MemberID = memberID;
        EmployeeId = employeeId;
        LoanDate = loanDate;
        IdealReturnDate = idealReturnDate;
        ReturnEmployeeID = returnEmployeeID;
        ReturnDate = returnDate;
        Description = description;
    }
    public void Edit(Guid bookId, string memberID, string employeeId, string loanDate, string idealReturnDate, string returnEmployeeID, string returnDate, string description)
    {
        BookId = bookId;
        MemberID = memberID;
        EmployeeId = employeeId;
        LoanDate = loanDate;
        IdealReturnDate = idealReturnDate;
        ReturnEmployeeID = returnEmployeeID;
        ReturnDate = returnDate;
        Description = description;
    }
}
