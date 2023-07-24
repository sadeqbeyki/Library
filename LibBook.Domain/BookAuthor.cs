using LibBook.Domain.AuthorAgg;
using LibBook.Domain.BookAgg;

namespace LibBook.Domain;

public class BookAuthor
{
    public int AuthorId { get; set; }
    public Author Author { get; set; }

    public int AuthorBookId { get; set; }
    public Book Book { get; set; }
}

