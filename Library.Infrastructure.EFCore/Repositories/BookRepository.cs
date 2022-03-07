using AppFramework.Infrastructure;
using Library.Application.Contracts.Book;
using Library.Domain.BookAgg;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Library.Infrastructure.EFCore.Repositories
{
    public class BookRepository : RepositoryBase<long, Book>, IBookRepository
    {
        private readonly LibraryContext _libraryContext;

        public BookRepository(LibraryContext libraryContext) : base(libraryContext)
        {
            _libraryContext = libraryContext;
        }

        public List<BookViewModel> GetBooks()
        {
            return _libraryContext.Books.Select(x => new BookViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        public Book GetBookWithCategory(long id)
        {
            return _libraryContext.Books.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
        }

        public EditBook GetDetails(long id)
        {
            return _libraryContext.Books.Select(x => new EditBook
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                CategoryId = x.CategoryId,
                Description = x.Description
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<BookViewModel> Search(BookSearchModel searchModel)
        {
            var query = _libraryContext.Books.Include(x => x.Category).Select(x => new BookViewModel
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                CategoryId = x.CategoryId,
                Category = x.Category.Name,
                CreationDate = x.CreationDate.ToString()
            });

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(x => x.Name.Contains(searchModel.Name));

            if (!string.IsNullOrWhiteSpace(searchModel.Code))
                query = query.Where(x => x.Code.Contains(searchModel.Code));

            if (searchModel.CategoryId != 0)
                query = query.Where(x => x.CategoryId == searchModel.CategoryId);

            return query.OrderByDescending(x => x.Id).ToList();
        }
    }
}
