using AppFramework.Domain;
using LMS.Domain.BookCategoryAgg;

namespace LMS.Domain.BookAgg;

public class Book : BaseEntity
{
    public string Title { get; set; }
    public string ISBN { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }

    public Guid CategoryId { get; set; }
    public BookCategory Category { get; private set; }
    //public List<BookCategory> Categories { get; private set; }

    //public long AuthorId { get; private set; }
    //public Author Author { get; set; }
    //public List<Author> Authors { get; set; }
    public List<AuthorBook> AuthorBooks { get; set; }


    //public long PublisherId { get; private set; }
    //public Publisher Publisher { get; set; }
    //public List<Publisher> Publishers { get; set; }
    public List<PublisherBook> PublisherBooks { get; set; }
        
    //public long TranslatorId { get; private set; }
    //public Translator Translator { get; set; }
    //public List<Translator> Translators { get; set; }
    public List<TranslatorBook> TranslatorBooks { get; set; }

    //public List<Borrow> Reservations { get; set; }

    public Guid PublisherId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid TranslatorId { get; set; }
    

    //public Book(string code, string title, string description, long categoryId)
    //{
    //    Code = code;
    //    Title = title;
    //    Description = description;
    //    CategoryId = categoryId;
    //}
}
