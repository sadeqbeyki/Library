namespace LMS.Contracts.Borrow;

public class BorrowDto
{
    public long Id { get; set; }
    public int BookId { get; set; }
    public string MemberID { get; set; }
    public string EmployeeId { get; set; }
    //public DateTime LendDate { get; set; }
    public DateTime IdealReturnDate { get; set; }
    public string ReturnEmployeeID { get; set; }
    public DateTime ReturnDate { get; set; }
    public string Description { get; set; }
}
