using System.Collections.Generic;

namespace AppQuery.Contracts.BookCategory
{
    public interface IBookCategoryQuery
    {
        List<BookCategoryQueryModel> GetBookCategories();
        List<BookCategoryQueryModel> GetBookCategoriesWithBooks();
    }
}
