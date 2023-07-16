namespace LendBook.ApplicationContract.Rent;

public interface ICartService
{
    Cart Get();
    void Set(Cart cart);
}