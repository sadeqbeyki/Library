namespace LMS.Contracts.Lend;

public class LendDto
{
    public int Id { get; set; }
    public string MemberID { get; set; }
    public string EmployeeId { get; set; }
    public string LendDate { get; set; }
    public string IdealReturnDate { get; set; }
    public string ReturnEmployeeID { get; set; }
    public string ReturnDate { get; set; }
    public string Description { get; set; }
    public List<LendItemDto> Items { get; set; } = new List<LendItemDto>();

}
