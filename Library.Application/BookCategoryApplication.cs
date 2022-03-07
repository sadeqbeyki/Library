using AppFramework.Application;
using Library.Application.Contracts;
using Library.Domain.BookCategoryAgg;
using System.Collections.Generic;

namespace Library.Application
{
    public class BookCategoryApplication : IBookCategoryApplication
    {
        private readonly IBookCategoryRepository _bookCategoryRepository;

        public BookCategoryApplication(IBookCategoryRepository bookCategoryRepository)
        {
            _bookCategoryRepository = bookCategoryRepository;
        }

        public OperationResult Create(CreateBookCategory command)
        {
            var operation = new OperationResult();
            if (_bookCategoryRepository.Exists(x => x.Name == command.Name))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var bookCategory = new BookCategory(command.Name, command.Description);
            _bookCategoryRepository.Create(bookCategory);
            _bookCategoryRepository.SaveChanges();
            return operation.Succeeded();
        }

        public OperationResult Edit(EditBookCategory command)
        {
            var operation = new OperationResult();
            var bookCategory = _bookCategoryRepository.Get(command.Id);
            if (bookCategory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_bookCategoryRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            bookCategory.Edit(command.Name, command.Description);
            _bookCategoryRepository.SaveChanges();
            return operation.Succeeded();
        }

        public List<BookCategoryViewModel> GetBookCategories()
        {
            return _bookCategoryRepository.GetBookCategories();
        }

        public EditBookCategory GetDetails(long id)
        {
            return _bookCategoryRepository.GetDetails(id);
        }

        public List<BookCategoryViewModel> Search(BookCategorySearchModel searchModel)
        {
            return _bookCategoryRepository.Search(searchModel);
        }
    }
}
