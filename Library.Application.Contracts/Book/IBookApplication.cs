using AppFramework.Application;
using System.Collections.Generic;

namespace Library.Application.Contracts.Book
{
    public interface IBookApplication
    {
        OperationResult Create(CreateBook command);
        OperationResult Edit(EditBook command);
        EditBook GetDetails(long id);
        List<BookViewModel> Search(BookSearchModel searchModel);
        List<BookViewModel> GetBooks();
    }
}
