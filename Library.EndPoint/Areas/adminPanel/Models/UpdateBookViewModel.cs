using LMS.Contracts.Author;
using LMS.Contracts.Book;
using LMS.Contracts.BookCategoryContract;
using LMS.Contracts.Publisher;
using LMS.Contracts.Translator;

namespace Library.EndPoint.Areas.adminPanel.Models;

public class UpdateBookViewModel
{
    public BookViewModel Book { get; set; } = new BookViewModel();

    public List<BookCategoryDto> BookCategories { get; set; } = new List<BookCategoryDto>();
    public List<AuthorDto> Authors { get; set; } = new List<AuthorDto>();
    public List<PublisherDto> Publishers { get; set; } = new List<PublisherDto>();
    public List<TranslatorDto> Translators { get; set; } = new List<TranslatorDto>();

}
