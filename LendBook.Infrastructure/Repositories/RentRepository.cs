using AppFramework.Application;
using AppFramework.Domain;
using LendBook.ApplicationContract;
using LendBook.ApplicationContract.Rent;
using LendBook.Domain.RentAgg;
using LI.Infrastructure;
using LMS.Infrastructure;

namespace LendBook.Infrastructure.Repositories;

public class RentRepository : Repository<Rent>, IRentRepository
{
    private readonly LendDbContext _lendContext;
    private readonly LiIdentityDbContext _userContext;
    private readonly BookDbContext _bookDbContext;

    public RentRepository(LendDbContext lendContext, LiIdentityDbContext userContext) : base(lendContext)
    {
        _lendContext = lendContext;
        _userContext = userContext;
    }

    public double GetAmountBy(Guid id)
    {
        var rent = _lendContext.Rents
            .Select(x => new { x.PayAmount, x.Id })
            .FirstOrDefault(x => x.Id == id);
        if (rent != null)
            return rent.PayAmount;
        return 0;
    }

    public List<RentItemViewModel> GetItems(Guid rentId)
    {
        var books = _bookDbContext.Books.Select(x => new { x.Id, x.Title }).ToList();
        var rent = _lendContext.Rents.FirstOrDefault(x => x.Id == rentId);
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
        var query = _lendContext.Rents.Select(x => new RentViewModel
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
