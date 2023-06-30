namespace LMS.Contracts.Rent;

public interface ICartService
{
    Cart Get();
    void Set(Cart cart);
}