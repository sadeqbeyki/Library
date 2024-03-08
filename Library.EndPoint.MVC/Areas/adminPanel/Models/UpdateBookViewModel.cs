using Library.Application.DTOs.BookCategory;
using Library.Application.DTOs.Books;

namespace Library.EndPoint.MVC.Areas.adminPanel.Models;

public class UpdateBookViewModel
{
    public BookViewModel Book { get; set; }
    public List<BookCategoryDto> BookCategories { get; set; }
    public List<string> Authors { get; set; }
    public List<string> Publishers { get; set; }
    public List<string> Translators { get; set; }
}
