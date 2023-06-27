namespace LMS.Domain.LoanAgg;

public class Loan
{
    public  int Id { get; set; }
    public Guid BookId { get; set; }
    public string EmployeeId { get; set; }
    public string LoanDate { get; set; }
    public string IdealReturnDate { get; set;}
    public string ReturnEmployeeID { get; set;}
    public string ReturnDate { get; set;}
    public string Description { get; set;}
}
