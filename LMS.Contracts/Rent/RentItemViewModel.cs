namespace LMS.Contracts.Rent;

public class RentItemViewModel
{
    public long Id { get; set; }
    public long BookId { get; set; }
    public string Book { get; set; }
    public int Count { get; set; }
    public double UnitPrice { get; set; }
    public int DiscountRate { get; set; }
    public long RentId { get; set; }
}