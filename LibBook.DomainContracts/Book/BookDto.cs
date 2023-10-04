using LibBook.DomainContracts.Author;

namespace LibBook.DomainContracts.Book;

public class BookDto
{
    public string Title { get; set; }
    public string ISBN { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }

    public int CategoryId { get; set; }
    public string Category { get; set; }

    public List<string> Publishers { get; set; }
    public List<string> Authors { get; set; }
    public List<string> Translators { get; set; }

}