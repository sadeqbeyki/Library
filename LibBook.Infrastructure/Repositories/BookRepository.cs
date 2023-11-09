using AppFramework.Domain;
using LibBook.Domain.BookAgg;
using LibBook.DomainContracts.Book;
using Microsoft.EntityFrameworkCore;

namespace LibBook.Infrastructure.Repositories;

public class BookRepository : Repository<Book, int>, IBookRepository
{
    private readonly BookDbContext _bookDbContext;
    public BookRepository(BookDbContext dbContext, BookDbContext bookDbContext) : base(dbContext)
    {
        _bookDbContext = bookDbContext;
    }

    public async Task<List<BookViewModel>> GetBooks()
    {
        return await _bookDbContext.Books.Select(x => new BookViewModel
        {
            Id = x.Id,
            Title = x.Title
        }).ToListAsync();
    }

    public List<BookViewModel> Search(BookSearchModel searchModel)
    {
        var query = _bookDbContext.Books
        .Include(x => x.Category)
        .Select(x => new BookViewModel
        {
            Id = x.Id,
            Title = x.Title,
            ISBN = x.ISBN,
            Code = x.Code,
            Description = x.Description,
            Category = x.Category.Name,
            CategoryId = x.CategoryId,
        });

        if (!string.IsNullOrWhiteSpace(searchModel.Title))
            query = query.Where(x => x.Title.Contains(searchModel.Title));

        if (!string.IsNullOrWhiteSpace(searchModel.Code))
            query = query.Where(x => x.Code.Contains(searchModel.Code));

        if (searchModel.CategoryId != 0)
            query = query.Where(x => x.CategoryId == searchModel.CategoryId);

        return query.OrderByDescending(x => x.Id).ToList();
    }
}
