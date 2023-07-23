using AppFramework.Application;
using AppFramework.Domain;
using LibBook.Domain.LendAgg;
using LibBook.DomainContracts.Lend;
using LibIdentity.Infrastructure;

namespace LibBook.Infrastructure.Repositories;

public class LendRepository : Repository<Lend, int>, ILendRepository
{
    private readonly BookDbContext _bookDbContext;
    private readonly IdentityDbContext _liIdentityDbContext;
    public LendRepository(BookDbContext dbContext, IdentityDbContext liIdentityDbContext) : base(dbContext)
    {
        _bookDbContext = dbContext;
        _liIdentityDbContext = liIdentityDbContext;
    }



    //public LendItem GetBookBy(int bookId)
    //{
    //    return _bookDbContext.Lends.FirstOrDefault(b => b.BookId == bookId);
    //}

    public List<LendItemDto> GetItems(int lendId)
    {
        var books = _bookDbContext.Books.Select(x => new { x.Id, x.Title }).ToList();
        var lend = _bookDbContext.Lends.FirstOrDefault(x => x.Id == lendId);
        if (lend == null)
            return new List<LendItemDto>();

        var items = lend.Items.Select(x => new LendItemDto
        {
            Id = x.Id,
            Count = x.Count,
            LendId = x.LendId,
            BookId = x.BookId,
        }).ToList();

        foreach (var item in items)
        {
            item.Book = books.FirstOrDefault(x => x.Id == item.BookId)?.Title;
        }

        return items;
    }

    public List<LendDto> Search(LendSearchModel searchModel)
    {
        var accounts = _liIdentityDbContext.Users.Select(x => new { x.Id, x.LastName }).ToList();
        var query = _bookDbContext.Lends.Select(x => new LendDto
        {
            Id = x.Id,
            MemberId = x.MemberId,
            LendDate = x.CreationDate
        });

        //query = query.Where(x => x.IsCanceled == searchModel.IsCanceled);

        if (searchModel.MemberId != null) query = query.Where(x => x.MemberId == searchModel.MemberId);

        var lends = query.OrderByDescending(x => x.Id).ToList();
        foreach (var lend in lends)
        {
            lend.MemberId = accounts.FirstOrDefault(x => x.Id == lend.MemberId)?.LastName;
            //lend.PaymentMethod = PaymentMethod.GetBy(lend.PaymentMethodId).Name;
        }

        return lends;
    }
}
