namespace LendBook.ApplicationContract.Rent;

public class RentItemViewModel
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string Book { get; set; }
    public int Count { get; set; }
    public double UnitPrice { get; set; }
    public int DiscountRate { get; set; }
    public long RentId { get; set; }
}