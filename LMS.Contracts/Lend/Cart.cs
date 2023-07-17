namespace LMS.Contracts.Lend;

public class Cart
{
    public List<CartItem> Items { get; set; }

    public Cart()
    {
        Items = new List<CartItem>();
    }

    public void Add(CartItem cartItem)
    {
        Items.Add(cartItem);
    }
}