using LMS.Contracts.Book;

namespace LMS.Contracts.Author;

public class AuthorBooksDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<BookDto> Books { get; set; }
}
