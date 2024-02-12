using AppFramework.Domain;
using LibBook.Domain.BookCategoryAgg;

namespace LibBook.Domain.BookAgg;

public class Book : BaseEntity
{
    public string Title { get; set; }
    public string ISBN { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }

    public int CategoryId { get; set; }
    public BookCategory Category { get; private set; }


    public List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    public List<BookPublisher> BookPublishers { get; set; } = new List<BookPublisher>();
    public List<BookTranslator> BookTranslators { get; set; } = new List<BookTranslator>();

    public Book(string title,
                string iSBN,
                string code,
                string description,
                int categoryId)
    {
        Title = title;
        ISBN = iSBN;
        Code = code;
        Description = description;
        CategoryId = categoryId;
    }
    public void Edit(string title, string iSBN, string code, string description, int categoryId)
    {
        Title = title;
        ISBN = iSBN;
        Code = code;
        Description = description;
        CategoryId = categoryId;
    }
}

