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
}
