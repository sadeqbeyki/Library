namespace LMS.Contracts.Rent;

public interface IRentApplication
{
    long PlaceRent(Cart cart);
    double GetAmountBy(long id);
    void Cancel(long id);
    string PaymentSucceeded(long rentId, long refId);
    List<RentItemViewModel> GetItems(long rentId);
    List<RentViewModel> Search(RentSearchModel searchModel);
}