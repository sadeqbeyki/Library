using AppFramework.Domain;
using LMS.Domain.BookCategoryAgg;

namespace LMS.Domain.BookAgg;

public class Book : BaseEntity
{
    public string Title { get; set; }
    public string ISBN { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }

    public int CategoryId { get; set; }
    public BookCategory Category { get; private set; }

    public int AuthorId { get; set; }
    public List<AuthorBook> AuthorBooks { get; set; }

    public int PublisherId { get; set; }
    public List<PublisherBook> PublisherBooks { get; set; }

    public int TranslatorId { get; set; }
    public List<TranslatorBook> TranslatorBooks { get; set; }

    public Book(string title, string iSBN, string code, string description,
        int categoryId, int authorId, int publisherId, int translatorId)
    {
        Title = title;
        ISBN = iSBN;
        Code = code;
        Description = description;
        CategoryId = categoryId;
        AuthorId = authorId;
        PublisherId = publisherId;
        TranslatorId = translatorId;
    }

    public void Edit(string title, string iSBN, string code, string description,
    int categoryId, int authorId, int publisherId, int translatorId)
    {
        Title = title;
        ISBN = iSBN;
        Code = code;
        Description = description;
        CategoryId = categoryId;
        AuthorId = authorId;
        PublisherId = publisherId;
        TranslatorId = translatorId;
    }
}

