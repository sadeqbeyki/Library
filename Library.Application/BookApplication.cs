using AppFramework.Application;
using Library.Application.Contracts.Book;
using Library.Domain.BookAgg;
using System.Collections.Generic;

namespace Library.Application
{
    public class BookApplication : IBookApplication
    {
        private readonly IBookRepository _bookRepository;

        public BookApplication(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public OperationResult Create(CreateBook command)
        {
            var operation = new OperationResult();
            if (_bookRepository.Exists(x => x.Name == command.Name))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var book = new Book(command.Code, command.Name, command.Description, command.CategoryId);
            _bookRepository.Create(book);
            _bookRepository.SaveChanges();
            return operation.Succeeded();
        }

        public OperationResult Edit(EditBook command)
        {
            var operation = new OperationResult();
            var book = _bookRepository.GetBookWithCategory(command.Id);
            if (book == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_bookRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            book.Edit(command.Code, command.Name, command.Description, command.CategoryId);
            _bookRepository.SaveChanges();
            return operation.Succeeded();
        }

        public List<BookViewModel> GetBooks()
        {
            return _bookRepository.GetBooks();
        }

        public EditBook GetDetails(long id)
        {
            return _bookRepository.GetDetails(id);
        }

        public List<BookViewModel> Search(BookSearchModel searchModel)
        {
            return _bookRepository.Search(searchModel);
        }
    }
}
