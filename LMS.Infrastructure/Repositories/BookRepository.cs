using AppFramework.Domain;
using LMS.Contracts.Book;
using LMS.Domain.BookAgg;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public class BookRepository : Repository<Book>, IBookRepository
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
