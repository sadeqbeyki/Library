using AppFramework.Domain;

namespace LMS.Domain.RentAgg;

public class RentItem : BaseEntity
{
    public Guid BookId { get; private set; }
    public int Count { get; private set; }
    public double UnitPrice { get; private set; }
    public int DiscountRate { get; private set; }
    public long RentId { get; private set; }
    public Rent Rent { get; private set; }

    public RentItem(Guid bookId, int count, double unitPrice, int discountRate)
    {
        BookId = bookId;
        Count = count;
        UnitPrice = unitPrice;
        DiscountRate = discountRate;
    }
}