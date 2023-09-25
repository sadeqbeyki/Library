using AppFramework.Domain;
using LibBook.DomainContracts.Book;

namespace LibBook.Domain.BookAgg;

public interface IBookRepository : IRepository<Book, int>
{
    Task<List<BookViewModel>> GetBooks();
}
