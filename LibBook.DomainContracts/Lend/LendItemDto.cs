namespace LibBook.DomainContracts.Lend;

public class LendItemDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string Book { get; set; }
    public int Count { get; set; } = 1;
    public long LendId { get; set; }
}