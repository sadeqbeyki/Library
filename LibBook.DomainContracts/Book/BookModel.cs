namespace LibBook.DomainContracts.Book;

public class BookModel
{
    public int Id { get; set; }
    public int PublisherId { get; set; }
    public List<string> Publishers { get; set; }
    public int AuthorId { get; set; }
    public List<string> Authors { get; set; }
    public int TranslatorId { get; set; }
    public List<string> Translators { get; set; }
    public int CategoryId { get; set; }
    public string Category { get; set; }

    public string Title { get; set; }
    public string ISBN { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
}
