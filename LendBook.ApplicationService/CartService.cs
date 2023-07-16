
using LendBook.ApplicationContract.Rent;

namespace LendBook.ApplicationService;

public class CartService : ICartService
{
    public Cart Cart { get; set; }

    public Cart Get()
    {
        return Cart;
    }

    public void Set(Cart cart)
    {
        Cart = cart;
    }
}