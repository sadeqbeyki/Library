using AppFramework.Domain;
using LMS.Domain.BookCategoryAgg;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public class BookCategoryRepository : Repository<BookCategory>, IBookCategoryRepository
{
    public BookCategoryRepository(BookDbContext dbContext) : base(dbContext)
    {
    }
}
