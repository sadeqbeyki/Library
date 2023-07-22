using LMS.Domain.AuthorAgg;
using LMS.Domain.BookAgg;

namespace LMS.Domain;

public class AuthorBook
{
    public int AuthorId { get; set; }
    public Author Author { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; }
}

