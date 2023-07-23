using AppFramework.Domain;
using LibBook.Domain.BookAgg;
using LibBook.Domain.BookCategoryAgg;
using LibBook.DomainContracts.BookCategory;
using Microsoft.EntityFrameworkCore;

namespace LibBook.Infrastructure.Repositories;

public class BookCategoryRepository : Repository<BookCategory, int>, IBookCategoryRepository
{
    private readonly BookDbContext _dbContext;

    public BookCategoryRepository(BookDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<Book>> GetCategoryWithBooks(int id)
    {
        List<Book> books = await _dbContext.Books.Where(b => b.CategoryId == id).ToListAsync();
        return books;
    }
    public async Task<List<BookCategoryDto>> GetCategories()
    {
        return await _dbContext.BookCategories.Select(x => new BookCategoryDto
        {
            Id = x.Id,
            Name = x.Name
        }).ToListAsync();

    }
}
