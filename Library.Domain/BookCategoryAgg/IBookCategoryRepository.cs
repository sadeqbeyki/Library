using AppFramework.Domain;
using Library.Application.Contracts;
using System.Collections.Generic;

namespace Library.Domain.BookCategoryAgg
{
    public interface IBookCategoryRepository : IRepository<long, BookCategory>
    {
        EditBookCategory GetDetails(long id);
        List<BookCategoryViewModel> GetBookCategories();
        List<BookCategoryViewModel> Search(BookCategorySearchModel searchModel);
    }
}
