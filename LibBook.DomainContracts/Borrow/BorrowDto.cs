namespace LibBook.DomainContracts.Borrow;

public class BorrowDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string MemberId { get; set; }
    public string EmployeeId { get; set; }
    public DateTime LendDate { get; set; }
    public DateTime IdealReturnDate { get; set; }
    public string ReturnEmployeeId { get; set; }
    public DateTime ReturnDate { get; set; }
    public string Description { get; set; }
}
