using LibBook.DomainContracts.Author;
using LibBook.DomainContracts.BookCategory;
using LibBook.DomainContracts.Publisher;
using LibBook.DomainContracts.Translator;

namespace Library.EndPoint.Areas.adminPanel.Models;

public class CreateBookViewModel
{
    public int PublisherId { get; set; }
    public int AuthorId { get; set; }
    public int TranslatorId { get; set; }
    public int CategoryId { get; set; }

    public string Title { get; set; }
    public string ISBN { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }

    public List<BookCategoryDto> BookCategories { get; set; }
    public List<AuthorDto> Authors { get; set; } = new List<AuthorDto>();
    public List<PublisherDto> Publishers { get; set; } = new List<PublisherDto>();
    public List<TranslatorDto> Translators { get; set; } = new List<TranslatorDto>();

}
