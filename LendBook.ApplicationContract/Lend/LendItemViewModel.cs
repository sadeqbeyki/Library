namespace LendBook.ApplicationContract.Lend;

public class LendItemViewModel
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string Book { get; set; }
    public int Count { get; set; }
    public long LendId { get; set; }
}