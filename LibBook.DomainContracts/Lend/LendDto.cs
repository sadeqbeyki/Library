namespace LibBook.DomainContracts.Lend;

public class LendDto
{
    public int Id { get; set; }
    public string MemberId { get; set; }
    public string EmployeeId { get; set; }
    public DateTime LendDate { get; set; }
    public DateTime IdealReturnDate { get; set; }
    public string ReturnEmployeeId { get; set; }
    public DateTime ReturnDate { get; set; }
    public string Description { get; set; }
    public List<LendItemDto> Items { get; set; } = new List<LendItemDto>();

}
