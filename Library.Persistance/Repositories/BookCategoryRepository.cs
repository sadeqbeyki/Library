using AppFramework.Domain;
using Library.Application.DTOs.BookCategories;
using Library.Application.Interfaces;
using Library.Domain.Entities.BookAgg;
using Library.Domain.Entities.BookCategoryAgg;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistance.Repositories;

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
