using AppFramework.Infrastructure;
using Library.Application.Contracts;
using Library.Domain.BookCategoryAgg;
using System.Collections.Generic;
using System.Linq;

namespace Library.Infrastructure.EFCore.Repositories
{
    public class BookCategoryRepository : RepositoryBase<long, BookCategory>, IBookCategoryRepository
    {
        private readonly LibraryContext _libraryContext;

        public BookCategoryRepository(LibraryContext libraryContext) : base(libraryContext)
        {
            _libraryContext = libraryContext;
        }

        public List<BookCategoryViewModel> GetBookCategories()
        {
            return _libraryContext.BookCategories.Select(x => new BookCategoryViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        public EditBookCategory GetDetails(long id)
        {
            return _libraryContext.BookCategories.Select(x => new EditBookCategory
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<BookCategoryViewModel> Search(BookCategorySearchModel searchModel)
        {
            var query = _libraryContext.BookCategories.Select(x => new BookCategoryViewModel
            {
                Id = x.Id,
                Name = x.Name
            });
            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(x => x.Name.Contains(searchModel.Name));
            return query.OrderByDescending(x => x.Id).ToList();
        }
    }
}
