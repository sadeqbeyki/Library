using LMS.Domain.AuthorAgg;
using LMS.Domain.BookAgg;

namespace LMS.Domain;

public class AuthorBook
{
    public long AuthorId { get; set; }
    public Author Author { get; set; }

    public long BookId { get; set; }
    public Book Book { get; set; }
}

