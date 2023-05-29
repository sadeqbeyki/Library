using AppFramework.Domain;
using LMS.Domain.BookAgg;
using LMS.Domain.BookCategoryAgg;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public class BookCategoryRepository : Repository<BookCategory>, IBookCategoryRepository
{
    private readonly BookDbContext _dbContext;

    public BookCategoryRepository(BookDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<Book>> GetCategoryWithBooks(Guid id)
    {
        List<Book> books = await _dbContext.Books.Where(b => b.CategoryId == id).ToListAsync();
        return books;
    }
}
