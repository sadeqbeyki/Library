namespace LMS.Contracts.Borrow;

public class BorrowDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string MemberID { get; set; }
    public string EmployeeId { get; set; }
    public string LendDate { get; set; }
    public string IdealReturnDate { get; set; }
    public string ReturnEmployeeID { get; set; }
    public string ReturnDate { get; set; }
    public string Description { get; set; }
}
