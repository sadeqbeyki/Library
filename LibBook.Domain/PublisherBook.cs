using LibBook.Domain.BookAgg;
using LibBook.Domain.PublisherAgg;

namespace LibBook.Domain;

public class PublisherBook
{
    public int PublisherId { get; set; }
    public Publisher Publisher { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; }
}
