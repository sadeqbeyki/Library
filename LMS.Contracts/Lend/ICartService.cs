namespace LMS.Contracts.Lend
{
    public interface ICartService
    {
        Cart Get();
        void Set(Cart cart);
    }
}