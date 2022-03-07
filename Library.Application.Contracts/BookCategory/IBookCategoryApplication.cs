using AppFramework.Application;
using System.Collections.Generic;

namespace Library.Application.Contracts
{
    public interface IBookCategoryApplication
    {
        OperationResult Create(CreateBookCategory command);
        OperationResult Edit(EditBookCategory command);
        EditBookCategory GetDetails(long id);
        List<BookCategoryViewModel> GetBookCategories();
        List<BookCategoryViewModel> Search(BookCategorySearchModel searchModel);
    }
}
