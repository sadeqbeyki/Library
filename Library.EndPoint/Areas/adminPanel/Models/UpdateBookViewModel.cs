using LibBook.DomainContracts.Author;
using LibBook.DomainContracts.Book;
using LibBook.DomainContracts.BookCategory;
using LibBook.DomainContracts.Publisher;
using LibBook.DomainContracts.Translator;

namespace Library.EndPoint.Areas.adminPanel.Models;

public class UpdateBookViewModel
{
    public LibBook.DomainContracts.Book.UpdateBookViewModel Book { get; set; } = new LibBook.DomainContracts.Book.UpdateBookViewModel();

    public List<BookCategoryDto> BookCategories { get; set; } = new List<BookCategoryDto>();
    public List<AuthorDto> Authors { get; set; } = new List<AuthorDto>();
    public List<PublisherDto> Publishers { get; set; } = new List<PublisherDto>();
    public List<TranslatorDto> Translators { get; set; } = new List<TranslatorDto>();

}
