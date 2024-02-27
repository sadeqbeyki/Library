using Library.Domain.Entities.BookAgg;
using Library.Domain.Entities.PublisherAgg;

namespace Library.Domain.Entities.Common;

public class BookPublisher
{
    public int PublisherId { get; set; }
    public Publisher Publisher { get; set; }

    public int PublisherBookId { get; set; }
    public Book Book { get; set; }
}
