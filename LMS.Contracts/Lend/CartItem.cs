namespace LMS.Contracts.Lend;

public class CartItem
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int Count { get; set; }
    public bool IsInStock { get; set; }
}