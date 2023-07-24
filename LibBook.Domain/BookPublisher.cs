using LibBook.Domain.BookAgg;
using LibBook.Domain.PublisherAgg;

namespace LibBook.Domain;

public class BookPublisher
{
    public int PublisherId { get; set; }
    public Publisher Publisher { get; set; }

    public int PublisherBookId { get; set; }
    public Book Book { get; set; }
}
