using LibBook.DomainContracts.Book;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.EndPoint.Areas.adminPanel.Models;

public class BookWithCategoryViewModel
{
    public List<BookViewModel> Books { get; set; } = new List<BookViewModel>();
    public BookSearchModel SearchModel { get; set; } = new BookSearchModel();
    public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
}
