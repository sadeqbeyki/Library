using Library.Domain.Entities.AuthorAgg;
using Library.Domain.Entities.BookAgg;

namespace Library.Domain.Entities.Common;

public class BookAuthor
{
    public int AuthorId { get; set; }
    public Author Author { get; set; }

    public int AuthorBookId { get; set; }
    public Book Book { get; set; }
}

