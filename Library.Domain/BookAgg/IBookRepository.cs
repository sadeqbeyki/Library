using AppFramework.Domain;
using Library.Application.Contracts.Book;
using System.Collections.Generic;

namespace Library.Domain.BookAgg
{
    public interface IBookRepository : IRepository<long, Book>
    {
        EditBook GetDetails(long id);
        Book GetBookWithCategory(long id);
        List<BookViewModel> Search(BookSearchModel searchModel);
        List<BookViewModel> GetBooks();
    }
}
