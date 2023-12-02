namespace LibBook.DomainContracts.Borrow;

public class LoanSearchModel
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; }
    public string MemberName { get; set; }
    public string EmployeeName { get; set; }
}


