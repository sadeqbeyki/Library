using Library.Application.DTOs.Book;
using Library.Application.DTOs.BookCategory;

namespace Library.EndPoint.MVC.Areas.adminPanel.Models;

public class UpdateBookViewModel
{

}

public class EditBookViewModel
{
    public BookViewModel Book { get; set; }
    public List<BookCategoryDto> BookCategories { get; set; }
    public List<string> Authors { get; set; }
    public List<string> Publishers { get; set; }
    public List<string> Translators { get; set; }
}