using AppFramework.Domain;
using Library.Application.DTOs.Book;
using Library.Application.Interfaces;
using Library.Domain.Entities.BookAgg;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistance.Repositories;

public class BookRepositoryDapper : Repository<Book, int>, IBookRepository
{
    private readonly BookDbContext _bookDbContext;
    public BookRepositoryDapper(BookDbContext dbContext, BookDbContext bookDbContext) : base(dbContext)
    {
        _bookDbContext = bookDbContext;
    }

    public Task<BookViewModel> GetBookById(int id)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }
}