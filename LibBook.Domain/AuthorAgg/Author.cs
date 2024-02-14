using AppFramework.Domain;
using LibBook.Domain.BookAgg;

namespace LibBook.Domain.AuthorAgg;

public class Author : BaseEntity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<BookAuthor> AuthorBooks { get; set; } = new List<BookAuthor>();

    //public List<Book> GetAuthorBooks()
    //{
    //    var authorBooks = AuthorBooks.Select(ab => ab.Book).ToList();
    //    return authorBooks;
    //}
}
