using LibBook.Domain.BookCategoryAgg;
using LibBook.DomainContracts.Author;
using LibBook.DomainContracts.Book;
using LibBook.DomainContracts.BookCategory;
using LibBook.DomainContracts.Publisher;
using LibBook.DomainContracts.Translator;

namespace Library.EndPoint.MVC.Areas.adminPanel.Models;

public class UpdateBookViewModel
{
    public BookViewModel Book { get; set; } = new BookViewModel();

    public List<BookCategoryDto> BookCategories { get; set; } = new List<BookCategoryDto>();
    public List<AuthorDto> Authors { get; set; } = new List<AuthorDto>();
    public List<PublisherDto> Publishers { get; set; } = new List<PublisherDto>();
    public List<TranslatorDto> Translators { get; set; } = new List<TranslatorDto>();

}

public class EditBookViewModel
{
    public BookViewModel Book { get; set; }
    public List<BookCategoryDto> BookCategories { get; set; }
    public List<string> Authors { get; set; }
    public List<string> Publishers { get; set; }
    public List<string> Translators { get; set; }
}