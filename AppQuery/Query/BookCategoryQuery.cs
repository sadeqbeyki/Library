using AppQuery.Contracts.Book;
using AppQuery.Contracts.BookCategory;
using Library.Domain.BookAgg;
using Library.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AppQuery.Query
{
    public class BookCategoryQuery : IBookCategoryQuery
    {
        private readonly LibraryContext _libraryContext;

        public BookCategoryQuery(LibraryContext libraryContext)
        {
            _libraryContext = libraryContext;
        }

        public List<BookCategoryQueryModel> GetBookCategories()
        {
            return _libraryContext.BookCategories.Select(x => new BookCategoryQueryModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        public List<BookCategoryQueryModel> GetBookCategoriesWithBooks()
        {
            var categories = _libraryContext.BookCategories.Include(x => x.Books).ThenInclude(x => x.Category)
                .Select(x => new BookCategoryQueryModel
                {
                    Id=x.Id,
                    Name=x.Name,
                    Books=MapBooks(x.Books)
                }).OrderByDescending(x => x.Id).ToList();
            return categories;
        }
        private static List<BookQueryModel> MapBooks(List<Book> products)
        {
            return products.Select(p => new BookQueryModel
            {
                Id = p.Id,
                Category = p.Category.Name,
                Name = p.Name,
            }).ToList();
        }
    }
}
