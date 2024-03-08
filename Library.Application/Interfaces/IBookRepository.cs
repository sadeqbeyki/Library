using AppFramework.Domain;
using Library.Application.DTOs.Books;
using Library.Domain.Entities.BookAgg;

namespace Library.Application.Interfaces;

public interface IBookRepository : IRepository<Book, int>
{
    Task<List<BookViewModel>> GetBooks();
    List<BookViewModel> Search(BookSearchModel searchModel);
}
