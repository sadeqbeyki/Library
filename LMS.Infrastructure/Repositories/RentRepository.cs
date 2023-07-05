using AppFramework.Application;
using AppFramework.Domain;
using LI.Infrastructure;
using LMS.Contracts;
using LMS.Contracts.Rent;
using LMS.Domain.RentAgg;

namespace LMS.Infrastructure.Repositories;

public class RentRepository : Repository<Rent>, IRentRepository
{
    private readonly BookDbContext _bookContext;
    private readonly LiIdentityDbContext _userContext;

    public RentRepository(BookDbContext bookContext, LiIdentityDbContext userContext) : base(bookContext)
    {
        _bookContext = bookContext;
        _userContext = userContext;
    }

    public double GetAmountBy(Guid id)
    {
        var rent = _bookContext.Rents
            .Select(x => new { x.PayAmount, x.Id })
            .FirstOrDefault(x => x.Id == id);
        if (rent != null)
            return rent.PayAmount;
        return 0;
    }

    public List<RentItemViewModel> GetItems(Guid rentId)
    {
        var books = _bookContext.Books.Select(x => new { x.Id, x.Title }).ToList();
        var rent = _bookContext.Rents.FirstOrDefault(x => x.Id == rentId);
        if (rent == null)
            return new List<RentItemViewModel>();

        var items = rent.Items.Select(x => new RentItemViewModel
        {
            Id = x.Id,
            Count = x.Count,
            DiscountRate = x.DiscountRate,
            RentId = x.RentId,
            BookId = x.BookId,
            UnitPrice = x.UnitPrice
        }).ToList();

        foreach (var item in items)
        {
            item.Book = books.FirstOrDefault(x => x.Id == item.BookId)?.Title;
        }

        return items;
    }

    public List<RentViewModel> Search(RentSearchModel searchModel)
    {
        var accounts = _userContext.Users.Select(x => new { x.Id, x.FirstName }).ToList();
        var query = _bookContext.Rents.Select(x => new RentViewModel
        {
            Id = x.Id,
            AccountId = x.AccountId,
            DiscountAmount = x.DiscountAmount,
            IsCanceled = x.IsCanceled,
            IsPaid = x.IsPaid,
            IssueTrackingNo = x.IssueTrackingNo,
            PayAmount = x.PayAmount,
            PaymentMethodId = x.PaymentMethod,
            RefId = x.RefId,
            TotalAmount = x.TotalAmount,
            CreationDate = x.CreationDate.ToFarsi()
        });

        query = query.Where(x => x.IsCanceled == searchModel.IsCanceled);

        if (searchModel.AccountId != null) query = query.Where(x => x.AccountId == searchModel.AccountId);

        var rents = query.OrderByDescending(x => x.Id).ToList();
        foreach (var rent in rents)
        {
            rent.AccountFullName = accounts.FirstOrDefault(x => x.Id == rent.AccountId)?.FirstName;
            rent.PaymentMethod = PaymentMethod.GetBy(rent.PaymentMethodId).Name;
        }

        return rents;
    }

}
