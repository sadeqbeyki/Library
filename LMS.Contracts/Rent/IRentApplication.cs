namespace LMS.Contracts.Rent;

public interface IRentApplication
{
    Guid PlaceRent(Cart cart);
    double GetAmountBy(Guid id);
    Task Cancel(Guid id);
    Task<string> PaymentSucceeded(Guid rentId, long refId);
    List<RentItemViewModel> GetItems(Guid rentId);
    List<RentViewModel> Search(RentSearchModel searchModel);
}