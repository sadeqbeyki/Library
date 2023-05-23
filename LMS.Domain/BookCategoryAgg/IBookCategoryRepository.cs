using AppFramework.Domain;

namespace LMS.Domain.BookCategoryAgg;

public interface IBookCategoryRepository:IRepository<BookCategory>
{
    //BookCategory GetCategoryWithBooks(int id);
}
